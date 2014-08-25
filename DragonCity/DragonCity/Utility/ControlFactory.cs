using System.Net.Mime;
using Microsoft.Xna.Framework.Graphics;
using tranduytrung.DragonCity.Constant;
using tranduytrung.DragonCity.ContextMenu;
using tranduytrung.DragonCity.Control;
using tranduytrung.DragonCity.Model;
using tranduytrung.Xna.Core;

namespace tranduytrung.DragonCity.Utility
{
    public static class ControlFactory
    {
        public static Button CreateButton(string text, SpriteFont font, Texture2D normalBackground, Texture2D hoverBackground, Texture2D pressBackground)
        {
            var button = new Button();
            button.Width = ControlConfig.ButtonWidth;
            button.Height = ControlConfig.ButtonHeight;
            var backSprite = new Sprite(new SingleSpriteSelector(normalBackground)) {SpriteMode = SpriteMode.Fit};
            button.Background = backSprite;
            button.NormalBackground = backSprite;

            backSprite = new Sprite(new SingleSpriteSelector(hoverBackground)) {SpriteMode = SpriteMode.Fit};
            button.HoverBackground = backSprite;

            backSprite = new Sprite(new SingleSpriteSelector(pressBackground)) {SpriteMode = SpriteMode.Fit};
            button.PressBackground = backSprite;

            var buttonText = new SpriteText(font) {Text = text};
            buttonText.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            buttonText.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Center);
            button.PresentableContent = buttonText;

            return button;
        }

        public static ToggleButton CreateServiceButton(IService service)
        {
            var button = new ToggleButton
            {
                Width = ControlConfig.ToggleButtonWidth,
                Height = ControlConfig.ToggleButtonHeight
            };
            button.Margin = new Margin(0, 24);
            ContextMenuExtension.SetContextMenu(button, service.ContextMenu);

            var backSprite = new Sprite(new SingleSpriteSelector(Textures.ToggleButtonNormal)) { SpriteMode = SpriteMode.Fit };
            button.Background = backSprite;
            button.NormalBackground = backSprite;

            backSprite = new Sprite(new SingleSpriteSelector(Textures.ToggleButtonHover)) { SpriteMode = SpriteMode.Fit };
            button.HoverBackground = backSprite;

            backSprite = new Sprite(new SingleSpriteSelector(Textures.ToggleButtonSelected)) { SpriteMode = SpriteMode.Fit };
            button.ToggledBackground = backSprite;

            var buttonContent = service.Logo;
            buttonContent.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            buttonContent.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Center);
            button.PresentableContent = buttonContent;

            return button;
        }
    }
}
