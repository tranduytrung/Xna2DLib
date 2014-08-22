using Microsoft.Xna.Framework.Graphics;
using tranduytrung.DragonCity.Control;
using tranduytrung.Xna.Control;
using tranduytrung.Xna.Core;

namespace tranduytrung.DragonCity.Utility
{
    public static class ControlFactory
    {
        public static SpriteButton CreateButton(Texture2D background, SpriteFont font, string text)
        {
            var button = new SpriteButton();

            var buttonContent = new Canvas();
            button.PresentableContent = buttonContent;

            var buttonSprite = new Sprite(new SingleSpriteSelector(background));
            buttonSprite.SpriteMode = SpriteMode.Fit;
            buttonSprite.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            buttonContent.Children.Add(buttonSprite);

            var textContainer = new ContentPresenter();
            buttonContent.Children.Add(textContainer);

            var buttonText = new SpriteText(font);
            buttonText.Text = text;
            buttonText.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            buttonText.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Center);
            textContainer.PresentableContent = buttonText;

            return button;
        }
    }
}
