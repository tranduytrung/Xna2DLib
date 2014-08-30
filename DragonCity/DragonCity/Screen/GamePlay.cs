using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tranduytrung.DragonCity.Constant;
using tranduytrung.DragonCity.Control;
using tranduytrung.DragonCity.Repository;
using tranduytrung.DragonCity.Template;
using tranduytrung.DragonCity.Utility;
using tranduytrung.Xna.Control;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Engine;
using tranduytrung.Xna.Map;

namespace tranduytrung.DragonCity.Screen
{
    public class GamePlay : ComponentBase
    {
        private DrawableObject _selectedObject;

        private Canvas _canvas;

        private ScrollableView _mapView;

        public IsometricMap MapControl { get; private set; }
        public int Foods { get; private set; }

        private DockPanel _dockPanel;
        private StackPanel _servicePanel;
        private ContentPresenter _contextPanel;

        public GamePlay(Game game) : base(game)
        {
        }

        protected override void LoadContent()
        {
            #region Setting Canvas
            _canvas = new Canvas();
            PresentableContent = _canvas;
            #endregion

            #region Setup scrollable view
            _mapView = new ScrollableView { Decelerator = 1000 };
            _canvas.Children.Add(_mapView);
            #endregion

            #region Load Map
            var keyColorText = Textures.MapColorKey;
            var keyColor = new Color[keyColorText.Width*keyColorText.Height];
            keyColorText.GetData(keyColor);

            var grassText = Textures.TileGrass;
            var lushText = Textures.TileLush;
            var desertText = Textures.TileDesert;
            var cryogenicText = Textures.TileCryogenic;

            var mapTemplate = Textures.MapSagaland;

            MapControl = new IsometricMap(mapTemplate.Width, mapTemplate.Height, 128, 64, keyColor);
            MapControl.Width = (mapTemplate.Width - 1)*64;
            MapControl.Height = (mapTemplate.Height - 1)*32;
            _mapView.PresentableContent = MapControl;
            _mapView.FrameRect = new Rectangle(0, 0, MapControl.Width, MapControl.Height);

            MapControl.BuildTerain(mapTemplate, new Dictionary<Color, Texture2D>()
            {
                {new Color(0, 255, 0), grassText},
                {new Color(0, 128, 0), lushText},
                {new Color(255, 178, 127), desertText},
                {new Color(0, 0, 255), cryogenicText}
            });

            #endregion

            #region Setup dock panel
            _dockPanel = new DockPanel();
            _canvas.Children.Add(_dockPanel);
            #endregion

            #region Setup Shop Panel

            _servicePanel = new StackPanel { Orientation = StackOrientation.Vertical, BackgroundColor = ControlConfig.ShopPanelBackgroundColor};

            _servicePanel.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Stretch);
            _servicePanel.SetValue(DockPanel.DockProperty, Dock.Left);

            _dockPanel.Children.Add(_servicePanel);

            #endregion

            #region Create shop items

            foreach (var service in GameRepository.GetGamePlayServices())
            {
                var button = ControlFactory.CreateServiceButton(service);
                button.ToggleChanged += ToggleSelection;
                _servicePanel.Children.Add(button);
            }

            #endregion

            #region Init context pannel

            _contextPanel = new ContentPresenter
            {
                Width = 0,
                Height = 0,
                BackgroundColor = ControlConfig.ContextMenuBackgroundColor
            };
            _contextPanel.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _contextPanel.SetValue(DockPanel.DockProperty, Dock.Bottom);
            _dockPanel.Children.Add(_contextPanel);
            

            #endregion
        }

        void ToggleSelection(object sender, System.EventArgs e)
        {
            var newSelection = (ToggleButton)sender;
            if (newSelection.IsToggled)
            {
                Select(newSelection);
            }
            else
            {
                Unselect();
            }
        }


        public void Select(DrawableObject obj)
        {
            var oldSelection = _selectedObject as ToggleButton;
            if (oldSelection != null)
            {
                oldSelection.IsToggled = false;
            }

            Unselect();

            var template = TemplateExtension.GetTemplate(obj);
            var contextMenu = template.ContextMenu;

            if (contextMenu == null)
                _contextPanel.Width = _contextPanel.Height = 0;
            else
                _contextPanel.Width = _contextPanel.Height = int.MinValue;

            _contextPanel.PresentableContent = contextMenu;
            _selectedObject = obj;
            template.Start();
        }

        public void Unselect()
        {
            if (_selectedObject != null)
            {
                var oldTemplate = TemplateExtension.GetTemplate(_selectedObject);
                oldTemplate.End();
                _selectedObject = null;
                _contextPanel.PresentableContent = null;
                _contextPanel.Width = _contextPanel.Height = 0;
            }
        }

        public void AddFoods(int value)
        {
            if (value < 0)
                return;

            Foods += value;
        }

        /// <summary>
        /// Consume amount of foods
        /// </summary>
        /// <param name="value">amount of foods</param>
        /// <returns>true if success. Otherwise, false</returns>
        public bool ConsumeFoods(int value)
        {
            if (value < 0 || Foods < value)
                return false;

            Foods -= value;
            return true;
        }
    }
}
