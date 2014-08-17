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

        public DrawableObject PresentableContent { get; set; }

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
            var predictedWidth = Width != int.MinValue ? Width : availableSize.Width;
            var predictedHeight = Height != int.MinValue ? Height : availableSize.Height;

            PresentableContent.Measure(new Size(predictedWidth, predictedHeight));

            DesiredWidth = PresentableContent.DesiredWidth;
            DesiredHeight = PresentableContent.DesiredHeight;
        }

        public override void Arrange(Rectangle finalRectangle)
        {
            var contentMargin = (Margin) PresentableContent.GetValue(Panel.MarginProperty);
            var contentRect = new Rectangle(contentMargin.Left, contentMargin.Top,
                finalRectangle.Width - contentMargin.Right, finalRectangle.Height - contentMargin.Bottom);

            PresentableContent.Arrange(contentRect);

            base.Arrange(finalRectangle);
        }

        /// <summary>
        /// Update control state
        /// </summary>
        public override void Update()
        {

            // Update child
            PresentableContent.Update();

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

            PresentableContent.RenderTransform();
        }

        public override void PrepareVisual()
        {
            // Prepare child visual first
            PresentableContent.PrepareVisual();

            var graphicsDevice = GlobalGameState.GraphicsDevice;

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
            graphicsDevice.Clear(Color.Transparent);

            // Draw all children visual to this panel visual
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

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
            var destination = new Rectangle((int) (RelativeX + ActualTranslate.X), (int) (RelativeY + ActualTranslate.Y),
                (int) (ActualWidth*ActualScale.X), (int) (ActualHeight*ActualScale.Y));

            // Draw to outer batch
            spriteBatch.Draw(_renderTarget, destination, null, Color.White, ActualRotate, Vector2.Zero, SpriteEffects.None, 0);
        }

        protected virtual void OnRelease()
        {
        }

        public override bool MouseInput(Vector2 relativePoint)
        {
            var content = PresentableContent as InteractiveObject;
            if (content != null)
            {
                var pc = content;
                if (pc.MouseInput(new Vector2(relativePoint.X - RelativeX, relativePoint.Y - RelativeY)))
                    return true;
            }

            return base.MouseInput(relativePoint);
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
            EnableMouseEvent = true;
        }

        public override void Dispose()
        {
            PresentableContent.Dispose();
            base.Dispose();
        }
    }
}
