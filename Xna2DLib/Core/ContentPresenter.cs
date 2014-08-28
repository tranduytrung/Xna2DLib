using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tranduytrung.Xna.Engine;

namespace tranduytrung.Xna.Core
{
    public class ContentPresenter : InteractiveObject
    {
        private bool _clickSeasion;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _renderTarget;

        public Color BackgroundColor { get; set; }
        public DrawableObject Background { get; set; }
        public DrawableObject PresentableContent { get; set; }
        public Color TintingColor { get; set; }

        public event EventHandler<MouseEventArgs> Click;
        public event EventHandler<MouseEventArgs> Release;

        //public override DrawableObject HitTestCore(Vector2 relativePoint)
        //{
        //    var innerHitObj = PresentableContent.HitTestCore(relativePoint);
        //    if (innerHitObj != null) return innerHitObj;
        //    return base.HitTestCore(relativePoint);
        //}

        public override void Measure(Size availableSize)
        {
            DesiredWidth = Width != int.MinValue ? Width : availableSize.Width;
            DesiredHeight = Height != int.MinValue ? Height : availableSize.Height;

            if (PresentableContent != null)
            {
                PresentableContent.Measure(new Size(DesiredWidth, DesiredHeight));
                if (Width == int.MinValue)
                    DesiredWidth = PresentableContent.DesiredWidth;

                if (Height == int.MinValue)
                    DesiredHeight = PresentableContent.DesiredHeight;
            }

            if (Background != null)
                Background.Measure(new Size(DesiredWidth, DesiredHeight));
        }

        public override void Arrange(Rectangle finalRectangle)
        {
            if (PresentableContent != null)
            {
                var contentMargin = (Margin) PresentableContent.GetValue(Panel.MarginProperty);
                var contentRect = new Rectangle(contentMargin.Left, contentMargin.Top,
                    finalRectangle.Width - contentMargin.Right - contentMargin.Left,
                    finalRectangle.Height - contentMargin.Bottom - contentMargin.Top);

                PresentableContent.Arrange(AlignmentExtension.Align(PresentableContent, contentRect));
            }

            if (Background != null)
                Background.Arrange(new Rectangle(0, 0, finalRectangle.Width, finalRectangle.Height));

            base.Arrange(finalRectangle);
        }

        /// <summary>
        /// Update control state
        /// </summary>
        public override void Update()
        {
            // Update child
            if (PresentableContent != null)
                PresentableContent.Update();

            if (Background != null)
                Background.Update();

            // Process mouse release
            if (Input.IsLeftMouseButtonUp())
            {
                if (_clickSeasion)
                {
                    OnRelease();
                    if (Release != null)
                        Release.Invoke(this, new MouseEventArgs());
                }
                _clickSeasion = false;
            }
        }

        public override void RenderTransform()
        {
            base.RenderTransform();

            if (PresentableContent != null)
                PresentableContent.RenderTransform();

            if (Background != null)
                Background.RenderTransform();
        }

        public override void PrepareVisual()
        {
            if (ActualWidth <= 0 || ActualHeight <= 0) return;
            // Prepare child visual first
            if (PresentableContent != null)
                PresentableContent.PrepareVisual();

            if (Background != null)
                Background.PrepareVisual();

            var graphicsDevice = GameContext.GraphicsDevice;

            // create internal sprite batch if it is not existed
            if (_spriteBatch == null)
            {
                _spriteBatch = new SpriteBatch(graphicsDevice);
            }

            // if there are no render target, create a new one
            if (_renderTarget == null)
            {
                _renderTarget = new RenderTarget2D(graphicsDevice, ActualWidth, ActualHeight);
            }
            else
            {
                // if render target does not fit, clear the old and create another fit with it
                if (_renderTarget.Width != ActualWidth || _renderTarget.Height != ActualHeight)
                {
                    _renderTarget.Dispose();
                    _renderTarget = new RenderTarget2D(graphicsDevice, ActualWidth, ActualHeight);
                }
            }

            // Save old targets
            var oldRenderTargets = graphicsDevice.GetRenderTargets();
            // Set our render target
            graphicsDevice.SetRenderTarget(_renderTarget);

            // Fill with background Color
            graphicsDevice.Clear(BackgroundColor);

            // Draw all children visual to this panel visual
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            if (Background != null)
                Background.Draw(_spriteBatch);

            if (PresentableContent != null)
                PresentableContent.Draw(_spriteBatch);

            _spriteBatch.End();

            // Restore targets
            graphicsDevice.SetRenderTargets(oldRenderTargets);
        }

        /// <summary>
        /// Draw the PresentableContent
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ActualWidth <= 0 || ActualHeight <= 0) return;
            var destination = new Rectangle((int) (RelativeX + ActualTranslate.X), (int) (RelativeY + ActualTranslate.Y),
                (int) (ActualWidth*ActualScale.X), (int) (ActualHeight*ActualScale.Y));

            // Draw to outer batch
            spriteBatch.Draw(_renderTarget, destination, null, TintingColor, ActualRotate, Vector2.Zero, SpriteEffects.None, 0);
        }

        protected virtual void OnRelease()
        {
        }

        protected override bool MouseInput(Vector2 relativePoint)
        {
            if (base.MouseInput(relativePoint))
                return true;

            var content = PresentableContent as InteractiveObject;
            return content != null && content.MouseInputCore(new Vector2(relativePoint.X - RelativeX, relativePoint.Y - RelativeY));
        }

        protected override void OnLeftMouseButtonDown(ref bool interupt)
        {
            _clickSeasion = true;
        }

        protected override void OnLeftMouseButtonUp(ref bool interupt)
        {
            if (_clickSeasion)
            {
                OnClick();
                if (Click != null)
                    Click(this, new MouseEventArgs());
            }
        }

        protected virtual void OnClick() { }

        public ContentPresenter()
        {
            TintingColor = Color.White;
            BackgroundColor = Color.Transparent;
            EnableMouseEvent = true;
        }

        public override void Dispose()
        {
            if (PresentableContent != null)
                PresentableContent.Dispose();

            base.Dispose();
        }
    }
}
