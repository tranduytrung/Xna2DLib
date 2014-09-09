using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using tranduytrung.Xna.Animation;
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

            _frameRect.Width = FrameRect.Width <= 0 ? PresentableContent.DesiredWidth : FrameRect.Width;
            _frameRect.Height = FrameRect.Height <= 0 ? PresentableContent.DesiredHeight : FrameRect.Height;
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

        protected override bool OnLeftMouseButtonUp(Vector2 relativePoint)
        {
            var propagate = base.OnLeftMouseButtonUp(relativePoint);

            _enableMouseSpeedCalculation = false;

            var duration = (double)_mouseSpeed.Length()/Decelerator;
            var velocityVector = -_mouseSpeed;
            var unit = velocityVector == Vector2.Zero ? Vector2.Zero : Vector2.Normalize(velocityVector);
            var deceleratorVector = unit * -Decelerator;
            var xDestination = FrameRect.X + velocityVector.X * duration + deceleratorVector.X * duration * duration / 2;
            var yDestination = FrameRect.Y + velocityVector.Y * duration + deceleratorVector.Y * duration * duration / 2;            

            var xAnimation = new IntegerlAnimation(() => _frameRect.X, SetFrameX)
            {
                From = FrameRect.X,
                To = (int)xDestination,
                Duration = TimeSpan.FromSeconds(duration)
            };

            var yAnimation = new IntegerlAnimation(() => _frameRect.Y, SetFrameY)
            {
                From = FrameRect.Y,
                To = (int)yDestination,
                Duration = TimeSpan.FromSeconds(duration)
            };

            _floatingAnimation = new Storyboard();
            _floatingAnimation.Animations.Add(xAnimation);
            _floatingAnimation.Animations.Add(yAnimation);

            AnimationManager.BeginAnimation(_floatingAnimation);

            return propagate;
        }

        protected override bool OnLeftMouseButtonDown(Vector2 relativePoint)
        {
            var propagate = base.OnLeftMouseButtonDown(relativePoint);
            _enableMouseSpeedCalculation = true;

            if (AnimationManager.IsAnimating(_floatingAnimation))
            {
                AnimationManager.EndAnimation(_floatingAnimation);
            }

            return propagate;
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
                var gameTime = GameContext.GameTime;
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
