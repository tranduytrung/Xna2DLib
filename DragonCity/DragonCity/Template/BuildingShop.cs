using System;
using System.Collections.Generic;
using tranduytrung.DragonCity.Constant;
using tranduytrung.DragonCity.ContextMenu;
using tranduytrung.DragonCity.Control;
using tranduytrung.DragonCity.Model;
using tranduytrung.Xna.Control;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Map;

namespace tranduytrung.DragonCity.Template
{
    public class BuildingShop : ITemplate
    {
        private IsometricMap _map;
        private ToggleButton _selectedButton;
        public DrawableObject PresentableContent { get; private set; }
        public DrawableObject ContextMenu { get; private set; }
        public void Start()
        {
        }

        public void End()
        {
        }

        public void ApplyData(IsometricMap map, object data)
        {
            _map = map;
            SetupContextMenu(map, (IEnumerable<Building>)data);
        }

        public BuildingShop()
        {
            SetupPresentableContent();
        }

        private void SetupContextMenu(IsometricMap map, IEnumerable<Building> buildings)
        {
            var panel = new StackPanel();
            panel.Height = ControlConfig.ToggleButtonHeight + 12;

            foreach (var building in buildings)
            {
                var button = CreateShopItemButton(building);
                panel.Children.Add(button);
            }

            ContextMenu = panel;
        }

        private void SetupPresentableContent()
        {
            PresentableContent = new Sprite(new SingleSpriteSelector(Textures.Dragonarium)) {SpriteMode = SpriteMode.Fit};
        }

        private ToggleButton CreateShopItemButton(Building building)
        {
            var template = (ITemplate)Activator.CreateInstance(building.TemplateType);

            var button = new ToggleButton
            {
                Width = ControlConfig.ToggleButtonWidth,
                Height = ControlConfig.ToggleButtonHeight,
                Margin = new Margin(0, 12),
                Tag = building
            };

            var backSprite = new Sprite(new SingleSpriteSelector(Textures.ToggleButtonNormal)) { SpriteMode = SpriteMode.Fit };
            button.Background = backSprite;
            button.NormalBackground = backSprite;

            backSprite = new Sprite(new SingleSpriteSelector(Textures.ToggleButtonHover)) { SpriteMode = SpriteMode.Fit };
            button.HoverBackground = backSprite;

            backSprite = new Sprite(new SingleSpriteSelector(Textures.ToggleButtonSelected)) { SpriteMode = SpriteMode.Fit };
            button.ToggledBackground = backSprite;

            var buttonContent = template.PresentableContent;
            buttonContent.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            buttonContent.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Center);
            button.PresentableContent = buttonContent;

            button.ToggleChanged += SelectionChange;

            return button;
        }

        private void SelectionChange(object sender, EventArgs e)
        {
            var button = (ToggleButton)sender;
            if (button.IsToggled)
            {
                if (_selectedButton != null)
                {
                    _selectedButton.IsToggled = false;
                }

                var building = (Building) button.Tag;
                var template = (ITemplate)Activator.CreateInstance(building.TemplateType);
                var sprite = template.PresentableContent;
                sprite.Width = _map.CellWidth * 2;
                sprite.Height = _map.CellHeight * 2;
                sprite.SetValue(IsometricMap.DeploymentProperty, new FourDiamondsDeployment());
                _map.AddChild(sprite);
                _map.BindToMouse(sprite);
            }
            else
            {
                _selectedButton = null;
            }
        }
    }
}