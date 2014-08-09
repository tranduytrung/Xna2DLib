using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tranduytrung.Xna.Core;

namespace tranduytrung.Xna.Control
{
    public class Button : InteractiveObject
    {
        private bool _clickSeasion;

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
            var contentRect = new Rectangle(finalRectangle.X + contentMargin.Left, finalRectangle.Y + contentMargin.Top,
                finalRectangle.Width - contentMargin.Right, finalRectangle.Height - contentMargin.Bottom);

            PresentableContent.Arrange(contentRect);

            base.Arrange(finalRectangle);
        }

        /// <summary>
        /// Update control state
        /// </summary>
        public override void Update()
        {
            // base Update
            base.Update();

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
            PresentableContent.PrepareVisual();
        }

        /// <summary>
        /// Draw the PresentableContent
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            PresentableContent.Draw(spriteBatch);
        }

        protected virtual void OnRelease()
        {
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

        public Button()
        {
            EnableMouseEvent = true;
        }
    }
}
