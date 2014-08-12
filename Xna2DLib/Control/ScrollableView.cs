using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Engine;

namespace tranduytrung.Xna.Control
{
    public class ScrollableView : ContentPresenter
    {
        private Vector2 _mouseSpeed;
        private bool _enableMouseSpeedCalculation;
        private Storyboard _floatingAnimation;
        private Rectangle _frameRect;

        public Rectangle FrameRect
        {
            get { return _frameRect; }
            set { _frameRect = value; }
        }

        /// <summary>
        /// Decelerator in pixel per second
        /// </summary>
        public int Decelerator { get; set; }

        public override void Measure(Size availableSize)
        {
            DesiredWidth = Width != int.MinValue ? Width : availableSize.Width;
            DesiredHeight = Height != int.MinValue ? Height : availableSize.Height;

            PresentableContent.Measure(new Size(DesiredWidth, DesiredHeight));
        }

        public override void Arrange(Rectangle finalRectangle)
        {
            var contentRect = new Rectangle(-FrameRect.X, -FrameRect.Y,
                (int) (PresentableContent.DesiredWidth*((float) PresentableContent.DesiredWidth/FrameRect.Width)),
                (int) (PresentableContent.DesiredHeight*((float) PresentableContent.DesiredHeight/FrameRect.Height)));

            PresentableContent.Arrange(contentRect);

            ActualWidth = finalRectangle.Width;
            ActualHeight = finalRectangle.Height;
            RelativeX = finalRectangle.X;
            RelativeY = finalRectangle.Y;
        }

        protected override void OnLeftMouseButtonUp(ref bool interupt)
        {
            base.OnLeftMouseButtonUp(ref interupt);

            _enableMouseSpeedCalculation = false;

            var duration = (double)_mouseSpeed.Length()/Decelerator;
            var velocityVector = -_mouseSpeed;
            var unit = velocityVector == Vector2.Zero ? Vector2.Zero : Vector2.Normalize(velocityVector);
            var deceleratorVector = unit * -Decelerator;
            var xDestination = FrameRect.X + velocityVector.X * duration + deceleratorVector.X * duration * duration / 2;
            var yDestination = FrameRect.Y + velocityVector.Y * duration + deceleratorVector.Y * duration * duration / 2;            

            var xAnimation = new ContinuousAnimation()
            {
                From = FrameRect.X,
                To = xDestination,
                Duration = TimeSpan.FromSeconds(duration)
            };
            xAnimation.AnimationCallback += value => SetFrameX((int)(double) value);

            var yAnimation = new ContinuousAnimation()
            {
                From = FrameRect.Y,
                To = yDestination,
                Duration = TimeSpan.FromSeconds(duration)
            };
            yAnimation.AnimationCallback += value => SetFrameY((int)(double)value);

            _floatingAnimation = new Storyboard();
            _floatingAnimation.Animations.Add(xAnimation);
            _floatingAnimation.Animations.Add(yAnimation);

            BeginAnimation(_floatingAnimation);
        }

        protected override void OnLeftMouseButtonDown(ref bool interupt)
        {
            base.OnLeftMouseButtonDown(ref interupt);
            _enableMouseSpeedCalculation = true;

            if (IsAnimating(_floatingAnimation))
            {
                EndAnimation(_floatingAnimation);
            }
        }

        /// <summary>
        /// Update control state
        /// </summary>
        public override void Update()
        {
            // base Update
            base.Update();

            if (_enableMouseSpeedCalculation)
            {
                var gameTime = GlobalGameState.GameTime;
                var offset = Input.MouseOffset();

                if (Input.MouseState.LeftButton == ButtonState.Pressed)
                {
                    SetFrameX(_frameRect.X - (int)offset.X);
                    SetFrameY(_frameRect.Y -= (int)offset.Y);
                }
                _mouseSpeed = new Vector2(offset.X / (float)gameTime.ElapsedGameTime.TotalSeconds, offset.Y / (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }

        public override void RenderTransform()
        {
            base.RenderTransform();

            PresentableContent.RenderTransform();
        }

        public ScrollableView()
        {
            EnableMouseEvent = true;
        }

        private void SetFrameX(int x)
        {
            if (x > PresentableContent.ActualWidth - ActualWidth)
            {
                x = PresentableContent.ActualWidth - ActualWidth;
            }

            if (x < 0)
            {
                x = 0;
            }

            _frameRect.X = x;
        }

        private void SetFrameY(int y)
        {
            if (y > PresentableContent.ActualHeight - ActualHeight)
            {
                y = PresentableContent.ActualHeight - ActualHeight;
            }

            if (y < 0)
            {
                y = 0;
            }

            _frameRect.Y = y;
        }
    }
}
