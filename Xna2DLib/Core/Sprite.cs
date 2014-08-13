using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tranduytrung.Xna.Engine;

namespace tranduytrung.Xna.Core
{
    public sealed class Sprite : SpriteBase
    {
        private readonly ISpriteSelector _selector;
        private Rectangle? _frameBounds;

        public Sprite(ISpriteSelector selector)
        {
            _selector = selector;
            SelectorState = selector.GetFrane(new GameTime());
        }

        public SpriteMode SpriteMode { get; set; }

        public SpriteSelectorState SelectorState { get; private set; }

        public override void Measure(Size availableSize)
        {
            switch (SpriteMode)
            {
                case SpriteMode.Original:
                    DesiredWidth = Width == int.MinValue ? SelectorState.Texture.Width : Width;
                    DesiredHeight = Height == int.MinValue ? SelectorState.Texture.Height : Height;
                    break;
                case SpriteMode.Fit:
                    DesiredWidth = Width == int.MinValue ? availableSize.Width : Width;
                    DesiredHeight = Height == int.MinValue ? availableSize.Height : Height;
                    break;
                case SpriteMode.FitHorizontal:
                    DesiredWidth = Width == int.MinValue ? availableSize.Width : Width;
                    DesiredHeight = Height == int.MinValue ? (int)(SelectorState.Texture.Height * ((float)DesiredWidth / SelectorState.Texture.Width)) : Height;
                    break;
                case SpriteMode.FitVertical:
                    DesiredHeight = Height == int.MinValue ? availableSize.Height : Height;
                    DesiredWidth = Height == int.MinValue ? (int)(SelectorState.Texture.Width * ((float)DesiredHeight / SelectorState.Texture.Height)) : Width;
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Arrange(Rectangle finalRectangle)
        {
            ActualWidth = finalRectangle.Width;
            ActualHeight = finalRectangle.Height;
            RelativeX = finalRectangle.X;
            RelativeY = finalRectangle.Y;

            // Mode
            float ratio, hPart, vPart;
            switch (SpriteMode)
            {
                case SpriteMode.Original:
                    _frameBounds = new Rectangle(SelectorState.ClipBounds.X, SelectorState.ClipBounds.Y,
                        finalRectangle.Width < SelectorState.ClipBounds.Width ? finalRectangle.Width : SelectorState.ClipBounds.Width,
                        finalRectangle.Height < SelectorState.ClipBounds.Height ? finalRectangle.Height : SelectorState.ClipBounds.Height);
                    break;
                case SpriteMode.Fit:
                    hPart = Math.Max((float)finalRectangle.Width / DesiredWidth, 1);
                    vPart = Math.Max((float)finalRectangle.Height / DesiredHeight, 1);

                    _frameBounds = new Rectangle(SelectorState.ClipBounds.X, SelectorState.ClipBounds.Y,
                        (int)(SelectorState.ClipBounds.Width * hPart),
                        (int)(SelectorState.ClipBounds.Height * vPart));
                    break;
                case SpriteMode.FitHorizontal:
                    ratio = Math.Max((float)DesiredWidth / SelectorState.ClipBounds.Width, 1);
                    hPart = Math.Max((float)finalRectangle.Width / DesiredWidth, 1);
                    vPart = Math.Max((float)finalRectangle.Height / DesiredHeight, 1);

                    _frameBounds = new Rectangle(SelectorState.ClipBounds.X, SelectorState.ClipBounds.Y,
                        (int)(SelectorState.ClipBounds.Width * hPart),
                        (int)(SelectorState.ClipBounds.Height * vPart * ratio));
                    break;
                case SpriteMode.FitVertical:
                    ratio = Math.Max((float)DesiredHeight / SelectorState.ClipBounds.Height, 1);
                    hPart = Math.Max((float)finalRectangle.Width / DesiredWidth, 1);
                    vPart = Math.Max((float)finalRectangle.Height / DesiredHeight, 1);

                    _frameBounds = new Rectangle(SelectorState.ClipBounds.X, SelectorState.ClipBounds.Y,
                        (int)(SelectorState.ClipBounds.Width * ratio * hPart),
                        (int)(SelectorState.ClipBounds.Height * vPart));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Update object state
        /// </summary>
        public override void Update()
        {
            var state = _selector.GetFrane(GlobalGameState.GameTime);
            SelectorState = state;
        }

        /// <summary>
        /// Draw sprite to device
        /// </summary>
        /// <param name="spriteBatch">spite batch to draw to screen</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            var destination = new Rectangle((int)(RelativeX + ActualTranslate.X), (int)(RelativeY + ActualTranslate.Y),
                (int)(ActualWidth * ActualScale.X), (int)(ActualHeight * ActualScale.Y));
            
            // Draw to batch
            spriteBatch.Draw(SelectorState.Texture, destination, _frameBounds, TintingColor, ActualRotate, Vector2.Zero, SpriteEffects, 0);
        }
    }

    public enum SpriteMode
    {
        Original,
        Fit,
        FitHorizontal,
        FitVertical
    }
}
