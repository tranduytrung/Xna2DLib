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

        public static readonly AttachableProperty MarginProperty = AttachableProperty.RegisterProperty(typeof(Margin), new Margin(0));
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
            foreach (var child in Children)
            {
                child.Update();
            }
        }

        public override void PrepareVisual()
        {
            // No thing to draw
            if (ActualWidth <= 0 || ActualHeight <= 0) return;

            // Prepare children visual first
            foreach (var child in Children)
            {
                child.PrepareVisual();
            }

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
            // No thing to draw
            if (ActualWidth <= 0 || ActualHeight <= 0) return;

            // Draw to outer batch
            var destination = new Rectangle((int)(RelativeX + ActualTranslate.X), (int)(RelativeY + ActualTranslate.Y),
                (int)(ActualWidth * ActualScale.X), (int)(ActualHeight * ActualScale.Y));
            spriteBatch.Draw(_renderTarget, destination, null, Color.White, ActualRotate, Vector2.Zero, SpriteEffects.None, 0);
        }

        protected override bool MouseInput(Vector2 relativePoint)
        {
            if (base.MouseInput(relativePoint))
                return true;

            for (var i = Children.Count - 1; i >= 0; i--)
            {
                var interactiveObj = Children[i] as InteractiveObject;
                if (interactiveObj == null) continue;
                if (interactiveObj.MouseInputCore(new Vector2(relativePoint.X - RelativeX, relativePoint.Y - RelativeY)))
                    return true;
            }

            return false;
        }

        protected Panel()
        {
            EnableMouseEvent = true;
            BackgroundColor = Color.Transparent;
        }

        public override void Dispose()
        {
            foreach (var child in Children)
            {
                child.Dispose();
            }
            base.Dispose();
        }
    }
}
