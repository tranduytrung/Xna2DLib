using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Engine;

namespace tranduytrung.Xna.Control
{
    public class ScrollableView : InteractiveObject
    {
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _renderTarget;
        private DrawableObject _presentableContent;
        private Vector2 _mouseSpeed;
        private bool _enableMouseSpeedCalculation;
        private Storyboard _floatingAnimation;
        private Rectangle _frameRect;

        public DrawableObject PresentableContent
        {
            get { return _presentableContent; }
            set
            {
                _presentableContent = value;
                FrameRect = new Rectangle(0, 0, value.Width < 0 ? 0 : value.Width, value.Height < 0 ? 0 : value.Height);
            }
        }

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

            base.Arrange(finalRectangle);
        }

        protected override void OnLeftMouseButtonUp(ref bool interupt)
        {
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
            _enableMouseSpeedCalculation = true;

            if (IsAnimating(_floatingAnimation))
            {
                EndAnimation(_floatingAnimation);
            }
        }

        public override bool MouseInput(Vector2 relativePoint)
        {
            if (PresentableContent is InteractiveObject)
            {
                var pc = (InteractiveObject)PresentableContent;
                if (pc.MouseInput(new Vector2(relativePoint.X - RelativeX, relativePoint.Y - RelativeY)))
                    return true;
            }
            
            return base.MouseInput(relativePoint);
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
            var destination = new Rectangle((int)(RelativeX + ActualTranslate.X), (int)(RelativeY + ActualTranslate.Y),
                (int)(ActualWidth * ActualScale.X), (int)(ActualHeight * ActualScale.Y));

            // Draw to outer batch
            spriteBatch.Draw(_renderTarget, destination, null, Color.White, ActualRotate, Vector2.Zero, SpriteEffects.None, 0);
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
