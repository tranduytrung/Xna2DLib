using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
            

            foreach (var building in buildings)
            {
                var itemStack = new StackPanel {Orientation = StackOrientation.Vertical};
                itemStack.Margin = new Margin(12, 0);

                var button = CreateShopItemButton(building);
                button.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                itemStack.Children.Add(button);

                var pricePnael = new StackPanel();
                itemStack.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                itemStack.Children.Add(pricePnael);

                var costValue = new SpriteText(Fonts.ButtonFont);
                costValue.Text = building.BuyValue.ToString(CultureInfo.InvariantCulture);
                pricePnael.Children.Add(costValue);

                var goldIcon = new Sprite(new SingleSpriteSelector(Textures.Gold))
                {
                    SpriteMode = SpriteMode.Fit,
                    Height = ControlConfig.ResouceIconHeight,
                    Width = ControlConfig.ResouceIconWidth
                };
                pricePnael.Children.Add(goldIcon);

                panel.Children.Add(itemStack);
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
            var deployment = (IIsometricDeployable)building.PresentableContent.GetValue(IsometricMap.DeploymentProperty);
            deployment.Deploy(e.Coordinate, e.CellX, e.CellY);

            if (deployment.Formation.Any(cell => _map.GetChildren(cell.X, cell.Y).Any(item => TemplateExtension.GetTemplate(item) != null)))
            {
                return;
            }

            if (!DragonCity.GamePlay.ConsumeGolds(buildingPrototype.BuyValue))
                return;

            var model = buildingPrototype.Clone();

            TemplateExtension.SetTemplate(building.PresentableContent, building);
            building.ApplyData(model);

            
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