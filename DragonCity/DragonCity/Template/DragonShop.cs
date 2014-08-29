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
    public class DragonShop : ITemplate
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
            SetupContextMenu((IEnumerable<DragonBase>)data);
        }

        public DragonShop()
        {
            SetupPresentableContent();
        }

        private void SetupPresentableContent()
        {
            PresentableContent = new Sprite(new SingleSpriteSelector(Textures.Poo)) { SpriteMode = SpriteMode.Fit };
        }

        private void SetupContextMenu(IEnumerable<DragonBase> dragons)
        {
            var panel = new StackPanel();
            panel.Height = ControlConfig.ToggleButtonHeight + 12;

            foreach (var dragon in dragons)
            {
                var button = CreateShopItemButton(dragon);
                panel.Children.Add(button);
            }

            ContextMenu = panel;
        }

        private ToggleButton CreateShopItemButton(DragonBase building)
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
                var dragon = (DragonBase) button.Tag;
                var template = (ITemplate) Activator.CreateInstance(dragon.TemplateType);
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
            var dragonPrototype = (Dragon) _selectedButton.Tag;
            var dragon = (ITemplate) Activator.CreateInstance(dragonPrototype.TemplateType);
            var model = dragonPrototype.Clone();
            dragon.ApplyData(model);
            var deployment = (IIsometricDeployable)dragon.PresentableContent.GetValue(IsometricMap.DeploymentProperty);
            deployment.Deploy(e.Coordinate, e.CellX, e.CellY);
            _map.AddChild(dragon.PresentableContent);
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