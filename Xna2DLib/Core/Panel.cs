using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tranduytrung.Xna.Engine;

namespace tranduytrung.Xna.Core
{
    public abstract class Panel : InteractiveObject
    {
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _renderTarget;

        public static readonly AttachableProperty MarginProperty = AttachableProperty.RegisterProperty(typeof(Margin));
        public static readonly AttachableProperty HorizontalAlignmentProperty = AttachableProperty.RegisterProperty(typeof(HorizontalAlignment), HorizontalAlignment.Left);
        public static readonly AttachableProperty VerticalAlignmentProperty = AttachableProperty.RegisterProperty(typeof(VerticalAlignment), VerticalAlignment.Top);
        private readonly IList<DrawableObject> _children = new List<DrawableObject>();

        public IList<DrawableObject> Children
        {
            get { return _children; }
        }

        public Color BackgroundColor { get; set; }

        public override void RenderTransform()
        {
            base.RenderTransform();

            foreach (var child in Children)
            {
                child.RenderTransform();
            }
        }

        public override void Update()
        {
            base.Update();

            foreach (var child in Children)
            {
                child.Update();
            }
        }

        public override void PrepareVisual()
        {
            // Prepare children visual first
            foreach (var child in Children)
            {
                child.PrepareVisual();
            }

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
            graphicsDevice.Clear(BackgroundColor);

            // Draw all children visual to this panel visual
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            foreach (var child in Children)
            {
                child.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            // Restore targets
            graphicsDevice.SetRenderTargets(oldRenderTargets);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw to outer batch
            var destination = new Rectangle((int)(RelativeX + ActualTranslate.X), (int)(RelativeY + ActualTranslate.Y),
                (int)(ActualWidth * ActualScale.X), (int)(ActualHeight * ActualScale.Y));
            spriteBatch.Draw(_renderTarget, destination, null, Color.White, ActualRotate, Vector2.Zero, SpriteEffects.None, 0);
        }

        public override bool MouseInput(Vector2 relativePoint)
        {
            for (var i = Children.Count - 1; i >= 0; i--)
            {
                var interactiveObj = Children[i] as InteractiveObject;
                if (interactiveObj != null)
                {
                    if (interactiveObj.MouseInput(new Vector2(relativePoint.X - RelativeX, relativePoint.Y - RelativeY)))
                    {
                        return true;
                    }
                }
            }

            return base.MouseInput(relativePoint);
        }

        protected Rectangle Align(DrawableObject element, Rectangle availableRect)
        {
            int x, y, width, height;
            var hAlignment = (HorizontalAlignment)element.GetValue(HorizontalAlignmentProperty);
            var vAlignment = (VerticalAlignment)element.GetValue(VerticalAlignmentProperty);

            if (availableRect.Width <= element.DesiredWidth)
            {
                x = availableRect.X;
                width = availableRect.Width;
            }
            else
            {
                switch (hAlignment)
                {
                    case HorizontalAlignment.Left:
                        x = availableRect.X;
                        width = element.DesiredWidth;
                        break;
                    case HorizontalAlignment.Right:
                        x = availableRect.X + availableRect.Width - element.DesiredWidth;
                        width = element.DesiredWidth;
                        break;
                    case HorizontalAlignment.Center:
                        x = availableRect.X + (availableRect.Width - element.DesiredWidth) / 2;
                        width = element.DesiredWidth;
                        break;
                    case HorizontalAlignment.Stretch:
                        x = availableRect.X;
                        width = availableRect.Width;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (availableRect.Height <= element.DesiredHeight)
            {
                y = availableRect.Y;
                height = availableRect.Height;
            }
            else
            {
                switch (vAlignment)
                {
                    case VerticalAlignment.Top:
                        y = availableRect.Y;
                        height = element.DesiredHeight;
                        break;
                    case VerticalAlignment.Bottom:
                        y = availableRect.Y + availableRect.Height - element.DesiredHeight;
                        height = element.DesiredHeight;
                        break;
                    case VerticalAlignment.Center:
                        y = availableRect.Y + (availableRect.Height - element.DesiredHeight) / 2;
                        height = element.DesiredHeight;
                        break;
                    case VerticalAlignment.Stretch:
                        y = availableRect.Y;
                        height = availableRect.Height;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return new Rectangle(x, y, width, height);
        }

        protected override void OnLeftMouseButtonDown(ref bool interupt)
        {
            interupt = BackgroundColor.A != 0;
        }

        public override void OnLeftMouseButtonPressed(ref bool interupt)
        {
            interupt = BackgroundColor.A != 0;
        }

        protected override void OnLeftMouseButtonUp(ref bool interupt)
        {
            interupt = BackgroundColor.A != 0;
        }

        public Panel()
        {
            BackgroundColor = Color.Transparent;
        }
    }

    public enum HorizontalAlignment
    {
        Left, Right, Center, Stretch
    }

    public enum VerticalAlignment
    {
        Top, Bottom, Center, Stretch
    }
}
