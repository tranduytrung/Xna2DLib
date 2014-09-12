

using Dovahkiin.Constant;
using Dovahkiin.Control;
using Dovahkiin.Repository;
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


        public static ToggleButton CreateLeftPanelButton(int resouceId)
        {
            var button = new ToggleButton
            {
                Width = ControlConfig.ToggleButtonWidth,
                Height = ControlConfig.ToggleButtonHeight,
                Margin = new Margin(0, 12)
            };

            var backSprite = new Sprite(new SingleSpriteSelector(Resouces.GetTexture(Textures.ToggleButton))) { SpriteMode = SpriteMode.Fit };
            button.Background = backSprite;
            button.NormalBackground = backSprite;

            backSprite = new Sprite(new SingleSpriteSelector(Resouces.GetTexture(Textures.ToggleButtonHover))) { SpriteMode = SpriteMode.Fit };
            button.HoverBackground = backSprite;

            backSprite = new Sprite(new SingleSpriteSelector(Resouces.GetTexture(Textures.ToggleButtonSelected))) { SpriteMode = SpriteMode.Fit };
            button.ToggledBackground = backSprite;

            var icon = new Sprite(new SingleSpriteSelector(Resouces.GetTexture(resouceId)))
            {
                SpriteMode = SpriteMode.Fit
            };
            icon.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            icon.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Center);
            button.PresentableContent = icon;

            return button;
        }

        public static ToggleButton CreateInventoryButton(int resourceId)
        {
            var button = new ToggleButton
            {
                Width = ControlConfig.ToggleButtonWidth,
                Height = ControlConfig.ToggleButtonHeight,
                Margin = new Margin(0, 12)
            };

            var backSprite = new Sprite(new SingleSpriteSelector(Resouces.GetTexture(Textures.ToggleButton))) { SpriteMode = SpriteMode.Fit };
            button.Background = backSprite;
            button.NormalBackground = backSprite;

            backSprite = new Sprite(new SingleSpriteSelector(Resouces.GetTexture(Textures.ToggleButtonHover))) { SpriteMode = SpriteMode.Fit };
            button.HoverBackground = backSprite;

            backSprite = new Sprite(new SingleSpriteSelector(Resouces.GetTexture(Textures.ToggleButtonSelected))) { SpriteMode = SpriteMode.Fit };
            button.ToggledBackground = backSprite;

            Texture2D texture = null;
            object objectTexture = Resouces.GetObjectTexture(resourceId);
            if (objectTexture is ComplexTexture)
            {
                texture = (new ComplexMultipleSpriteSelector((ComplexTexture)objectTexture, State.stopped, Direction.se)).GetStillTexture();
            }
            else if (objectTexture is Texture2D)
            {
                texture = Resouces.GetTexture(resourceId);
            }
            else if (objectTexture is Texture2D[])
            {
                texture = Resouces.GetTextures(resourceId)[0];
            }

            var icon = new Sprite(new SingleSpriteSelector(texture))
            {
                SpriteMode = SpriteMode.Fit
            };
            icon.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            icon.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Center);
            button.PresentableContent = icon;

            return button;
        }
    }
}
