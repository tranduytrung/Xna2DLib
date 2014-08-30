using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        private ContentPresenter _contextPanel;
        private SpriteText _foodsText;
        private int _foods;
        private SpriteText _goldsText;
        private int _golds;
        private DrawableObject _selectedObject;
        private ScrollableView _mapView;
        private Cue _music;

        public IsometricMap MapControl { get; private set; }

        public int Foods
        {
            get { return _foods; }
            private set
            {
                if (_foods == value) return;
                _foods = value;
                UpdateFoodsText();
            }
        }

        public int Golds
        {
            get { return _golds; }
            private set
            {
                if (_golds == value) return;
                _golds = value;
                UpdateGoldsText();
            }
        }

        public GamePlay(Game game) : base(game)
        {


        }

        public override void OnTransitFrom()
        {
            if (_music.IsPaused)
                _music.Resume();
            else
                _music.Play();
        }

        public override void OnTransitTo()
        {
            _music.Pause();
        }

        protected override void LoadContent()
        {
            #region Setup music

            _music = Sounds.GetBackgroundMusic();

            #endregion

            #region Setting Canvas
            var canvas = new Canvas();
            PresentableContent = canvas;
            #endregion

            #region Setup scrollable view
            _mapView = new ScrollableView { Decelerator = 1000 };
            canvas.Children.Add(_mapView);
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
            var dockPanel = new DockPanel();
            canvas.Children.Add(dockPanel);
            #endregion

            #region Resouces Panel

            var resourceContainer = new ContentPresenter {BackgroundColor = ControlConfig.ResoucesPanelColor};
            resourceContainer.SetValue(DockPanel.DockProperty, Dock.Top);
            resourceContainer.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
            dockPanel.Children.Add(resourceContainer);

            var resourceStack = new StackPanel();
            resourceStack.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Right);
            resourceContainer.PresentableContent = resourceStack;

            _foodsText = new SpriteText(Fonts.ButtonFont);
            resourceStack.Children.Add(_foodsText);
            UpdateFoodsText();

            var foodIcon = new Sprite(new SingleSpriteSelector(Textures.Tomato))
            {
                SpriteMode = SpriteMode.Fit,
                Width = ControlConfig.ResouceIconWidth,
                Height = ControlConfig.ResouceIconHeight,
                Margin = new Margin(6, 0, 36, 0)
            };
            resourceStack.Children.Add(foodIcon);

            _goldsText = new SpriteText(Fonts.ButtonFont);
            resourceStack.Children.Add(_goldsText);
            UpdateGoldsText();

            var goldIcon = new Sprite(new SingleSpriteSelector(Textures.Gold))
            {
                SpriteMode = SpriteMode.Fit,
                Width = ControlConfig.ResouceIconWidth,
                Height = ControlConfig.ResouceIconHeight,
                Margin = new Margin(6, 0, 36, 0)
            };
            resourceStack.Children.Add(goldIcon);

            #endregion 

            #region Setup Shop Panel

            var servicePanel = new StackPanel { Orientation = StackOrientation.Vertical, BackgroundColor = ControlConfig.ShopPanelBackgroundColor};

            servicePanel.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Stretch);
            servicePanel.SetValue(DockPanel.DockProperty, Dock.Left);

            dockPanel.Children.Add(servicePanel);

            #endregion

            #region Create shop items

            foreach (var service in GameRepository.GetGamePlayServices())
            {
                var button = ControlFactory.CreateServiceButton(service);
                button.ToggleChanged += ToggleSelection;
                servicePanel.Children.Add(button);
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
            dockPanel.Children.Add(_contextPanel);
            

            #endregion

            #region Game config

            Golds = GameLogicConfig.StartGolds;
            Foods = GameLogicConfig.StartFoods;

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

        public void UpdateFoodsText()
        {
            _foodsText.Text = Foods.ToString(CultureInfo.InvariantCulture);
        }

        public void AddGolds(int value)
        {
            if (value < 0)
                return;

            Golds += value;
        }

        /// <summary>
        /// Consume amount of golds
        /// </summary>
        /// <param name="value">amount of golds</param>
        /// <returns>true if success. Otherwise, false</returns>
        public bool ConsumeGolds(int value)
        {
            if (value < 0 || Golds < value)
                return false;

            Golds -= value;
            return true;
        }

        private void UpdateGoldsText()
        {
            _goldsText.Text = Golds.ToString(CultureInfo.InvariantCulture);
        }

    }
}
