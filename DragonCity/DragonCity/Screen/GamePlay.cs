using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tranduytrung.DragonCity.Constant;
using tranduytrung.DragonCity.ContextMenu;
using tranduytrung.DragonCity.Control;
using tranduytrung.DragonCity.Repository;
using tranduytrung.DragonCity.Utility;
using tranduytrung.Xna.Control;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Engine;
using tranduytrung.Xna.Map;

namespace tranduytrung.DragonCity.Screen
{
    public class GamePlay : ComponentBase
    {
        private ToggleButton _selectedObject;

        private Canvas _canvas;

        private ScrollableView _mapView;
        private IsometricMap _mapControl;

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

            _mapControl = new IsometricMap(mapTemplate.Width, mapTemplate.Height, 128, 64, keyColor);
            _mapControl.Width = (mapTemplate.Width - 1)*64;
            _mapControl.Height = (mapTemplate.Height - 1)*32;
            _mapView.PresentableContent = _mapControl;
            _mapView.FrameRect = new Rectangle(0, 0, _mapControl.Width, _mapControl.Height);

            _mapControl.BuildTerain(mapTemplate, new Dictionary<Color, Texture2D>()
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

            _servicePanel = new StackPanel();
            var shopPanelContainer = new Canvas
            {
                Width = ControlConfig.ShopPanelWdith,
                BackgroundColor = ControlConfig.ShopPanelBackgroundColor
            };
            shopPanelContainer.Children.Add(_servicePanel);
            shopPanelContainer.SetValue(DockPanel.DockProperty, Dock.Left);
            _dockPanel.Children.Add(shopPanelContainer);

            #endregion

            #region Create shop items

            foreach (var service in GameRepository.GetGamePlayServices(_mapControl))
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
            var button = (ToggleButton) sender;
            if (button.IsToggled)
            {
                if (_selectedObject != null)
                {
                    _selectedObject.IsToggled = false;
                }
                _selectedObject = button;
                var contextMenu = ContextMenuExtension.GetContextMenu(_selectedObject);
                if (contextMenu == null)
                {
                    _contextPanel.PresentableContent = null;
                    _contextPanel.Width = _contextPanel.Height = 0;
                }
                else
                {
                    _contextPanel.PresentableContent = contextMenu;
                    _contextPanel.Width = _contextPanel.Height = int.MinValue;
                }
                
            }
            else
            {
                _selectedObject = null;
                _contextPanel.PresentableContent = null;
                _contextPanel.Width = _contextPanel.Height = 0;
            }
        }
    }
}
