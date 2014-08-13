using System.Collections.Generic;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using tranduytrung.Xna.Control;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Engine;
using tranduytrung.Xna.Map;

namespace GameMenu
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class MainMenuComponent : ComponentBase
    {
        private SpriteButton _startButton;
        private SpriteButton _quitButton;
        private Sprite _background;
        private Sprite _logo;
        private Canvas _canvas;
        private StackPanel _stackPanel;
        private ScrollableView _map;
        private DockPanel _dockPanel;
        private IsometricMap _mapControl;

        public MainMenuComponent(Game game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            var width = Game.Window.ClientBounds.Width;
            var height = Game.Window.ClientBounds.Height;

            _canvas = new Canvas();
            PresentableContent = _canvas;

            _background = new Sprite(new SingleSpriteSelector(Game.Content.Load<Texture2D>(@"images/clouds")));
            _background.Width = width*2;
            _background.Height = height*2;
            _background.SpriteMode = SpriteMode.Original;

            _map = new ScrollableView() { Decelerator = 400};
            _map.PresentableContent = _background;
            _map.FrameRect = new Rectangle(0,0,_background.Width, _background.Height);
            //PresentableContent = _map;
            _canvas.Children.Add(_map);

            _dockPanel = new DockPanel();
            _canvas.Children.Add(_dockPanel);

            _stackPanel = new StackPanel();
            _stackPanel.Orientation = StackOrientation.Vertical;
            _stackPanel.SetValue(DockPanel.DockProperty, Dock.Top);
            _stackPanel.SetValue(Panel.VerticalAlignmentProperty, VerticalAlignment.Center);
            _stackPanel.SetValue(Panel.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _stackPanel.Width = 400;
            _dockPanel.Children.Add(_stackPanel);

            var startTexture = Game.Content.Load<Texture2D>(@"images/menu/start");
            _startButton = new SpriteButton() { PresentableContent = new Sprite(new SingleSpriteSelector(startTexture)) };
            _startButton.SetValue(Panel.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _stackPanel.Children.Add(_startButton);

            var quitTexture = Game.Content.Load<Texture2D>(@"images/menu/quit");
            _quitButton = new SpriteButton() { PresentableContent = new Sprite(new SingleSpriteSelector(quitTexture)) };
            _quitButton.SetValue(Panel.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _stackPanel.Children.Add(_quitButton);


            var logoTexture = Game.Content.Load<Texture2D>(@"images/logo");
            _logo = new Sprite(new SingleSpriteSelector(logoTexture));
            _logo.SetValue(Canvas.XProperty, (width - logoTexture.Width) / 2);
            _logo.SetValue(Canvas.YProperty, (height - logoTexture.Height) / 5);
            _canvas.Children.Add(_logo);
            //Game.IsFixedTimeStep = false;
            //var keyColorText = Game.Content.Load<Texture2D>(@"images/colorKey128x64");
            //var keyColor = new Color[keyColorText.Width*keyColorText.Height];
            //keyColorText.GetData(keyColor);

            //var grassText = Game.Content.Load<Texture2D>(@"images/terrain/grass");
            //var lushText = Game.Content.Load<Texture2D>(@"images/terrain/lush");
            //var desertText = Game.Content.Load<Texture2D>(@"images/terrain/desert");
            //var cryogenicText = Game.Content.Load<Texture2D>(@"images/terrain/cryogenic");

            //var mapTemplate = Game.Content.Load<Texture2D>(@"images/sagaland");

            //_mapControl = new IsometricMap(mapTemplate.Width, mapTemplate.Height, 128, 64, keyColor);
            //_mapControl.Width = (mapTemplate.Width - 1)*64;
            //_mapControl.Height = (mapTemplate.Height - 1)*32;
            //_map.PresentableContent = _mapControl;

            //_mapControl.BuildTerain(mapTemplate, new Dictionary<Color, Texture2D>()
            //{
            //    {new Color(0, 255, 0), grassText},
            //    {new Color(0, 128, 0), lushText},
            //    {new Color(255, 178, 127), desertText},
            //    {new Color(0, 0, 255), cryogenicText}
            //});

            //var berry2 = new Sprite(new SingleSpriteSelector(keyColorText));
            //var deploy2 = new UnitDeployment();
            //deploy2.Deploy(new IsometricCoords(-100, -100));
            //berry2.SetValue(IsometricMap.DeploymentProperty, deploy2);
            //_mapControl.AddChild(berry2);
            //_mapControl.BindToMouse(berry2);
            
            base.LoadContent();
        }
    }
}
