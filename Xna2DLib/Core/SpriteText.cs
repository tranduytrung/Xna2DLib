using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace tranduytrung.Xna.Core
{
    public class SpriteText : SpriteBase
    {
        public SpriteText(SpriteFont font)
        {
            Font = font;
        }

        public string Text { get; set; }

        SpriteFont Font { get; set; }

        public override void Update()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw to batch
            spriteBatch.DrawString(Font, Text, new Vector2(RelativeX + ActualTranslate.X, RelativeY + ActualTranslate.Y), TintingColor, ActualRotate, Vector2.Zero,ActualScale, SpriteEffects, 0.0f);
        }

        public override void Measure(Size availableSize)
        {
            var stringSize = Font.MeasureString(Text);
            DesiredWidth = (int)stringSize.X;
            DesiredHeight = (int)stringSize.Y;
        }

        public override void Arrange(Rectangle finalRectangle)
        {
            RelativeX = finalRectangle.X;
            RelativeY = finalRectangle.Y;
            ActualWidth = Math.Min(finalRectangle.Width, DesiredWidth);
            ActualHeight = Math.Min(finalRectangle.Height, DesiredHeight);
        }
    }
}