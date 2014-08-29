using System;
using System.Collections.Generic;
using tranduytrung.DragonCity.Constant;
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
        private DrawableObject _mouseBindedObject;
        private ToggleButton _selectedButton;
        public DrawableObject PresentableContent { get; private set; }
        public DrawableObject ContextMenu { get; private set; }

        public void Start()
        {
        }

        public void End()
        {
            if (_mouseBindedObject != null)
                RemoveMouseObject();

            if (_selectedButton != null)
                _selectedButton.IsToggled = false;
        }

        public void ApplyData(object data)
        {
            _map = DragonCity.GamePlay.MapControl;
            SetupContextMenu((IEnumerable<Building>)data);
        }

        public BuildingShop()
        {
            SetupPresentableContent();
        }

        private void SetupContextMenu(IEnumerable<Building> buildings)
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
                _selectedButton = button;
                var building = (Building) button.Tag;
                var template = (ITemplate) Activator.CreateInstance(building.TemplateType);
                var sprite = template.PresentableContent;
                SetMouseObject(sprite);
            }
            else
            {
                _selectedButton = null;
                RemoveMouseObject();
            }
        }

        private void DeployBuilding(object sender, IsometricMouseEventArgs e)
        {
            var buildingPrototype = (Building) _selectedButton.Tag;
            var building = (ITemplate) Activator.CreateInstance(buildingPrototype.TemplateType);
            var model = buildingPrototype.Clone();
            building.ApplyData(model);

            var deployment = (IIsometricDeployable)building.PresentableContent.GetValue(IsometricMap.DeploymentProperty);
            deployment.Deploy(e.Coordinate, e.CellX, e.CellY);
            _map.AddChild(building.PresentableContent);
            _selectedButton.IsToggled = false;
        }

        private void CancelDeployment(object sender, MouseEventArgs e)
        {
            _selectedButton.IsToggled = false;
        }

        private void SetMouseObject(DrawableObject obj)
        {
            _mouseBindedObject = obj;
            _map.AddChild(obj);
            _map.BindToMouse(obj);
            _map.RightMouseButtonUp += CancelDeployment;
            _map.IsometricMouseClick += DeployBuilding;
        }

        private void RemoveMouseObject()
        {
            _map.RightMouseButtonUp -= CancelDeployment;
            _map.IsometricMouseClick -= DeployBuilding;
            _map.RemoveChild(_mouseBindedObject);
            _map.UnbindToMouse();
        }
    }
}