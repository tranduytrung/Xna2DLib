

using Dovahkiin.Constant;
using Dovahkiin.Control;
using Microsoft.Xna.Framework.Graphics;
using tranduytrung.Xna.Core;
namespace Dovahkiin.Utility
{
    public static class ControlFactory
    {
        public static Button CreateButton(string text, SpriteFont font, Texture2D normalBackground, Texture2D hoverBackground, Texture2D pressBackground)
        {
            var button = CreateFitContentButton(text, font, normalBackground, hoverBackground, pressBackground);
            button.Width = ControlConfig.ButtonWidth;
            button.Height = ControlConfig.ButtonHeight;

            return button;
        }

        public static Button CreateFitContentButton(string text, SpriteFont font, Texture2D normalBackground, Texture2D hoverBackground, Texture2D pressBackground)
        {
            var button = new Button();
            var backSprite = new Sprite(new SingleSpriteSelector(normalBackground)) { SpriteMode = SpriteMode.Fit };
            button.Background = backSprite;
            button.NormalBackground = backSprite;

            backSprite = new Sprite(new SingleSpriteSelector(hoverBackground)) { SpriteMode = SpriteMode.Fit };
            button.HoverBackground = backSprite;

            backSprite = new Sprite(new SingleSpriteSelector(pressBackground)) { SpriteMode = SpriteMode.Fit };
            button.PressBackground = backSprite;

            var buttonText = new SpriteText(font) { Text = text };
            buttonText.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            buttonText.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Center);
            button.PresentableContent = buttonText;

            return button;
        }


    }
}
