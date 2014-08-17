using System.Diagnostics;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tranduytrung.DragonCity.Control;
using tranduytrung.Xna.Control;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Engine;

namespace tranduytrung.DragonCity.Screen
{
    public class MainMenuScreen : ComponentBase
    {
        private Canvas _mainCanvas;
        private DockPanel _dockPanel;
        private Sprite _background;
        private Sprite _title;

        private StackPanel _mainMenuPanel;
        private SpriteButton _startButton;
        private SpriteButton _settingButton;
        private SpriteButton _quitButton;

        public MainMenuScreen(Game game) : base(game)
        {
        }

        protected override void LoadContent()
        {
            _mainCanvas = new Canvas();
            PresentableContent = _mainCanvas;

            _background = new Sprite(new SingleSpriteSelector(Game.Content.Load<Texture2D>(@"")));
            _mainCanvas.Children.Add(_background);

            _dockPanel = new DockPanel();
            _mainCanvas.Children.Add(_dockPanel);

            _title = new Sprite(new SingleSpriteSelector(Game.Content.Load<Texture2D>(@"")));
            _title.SetValue(DockPanel.DockProperty, Dock.Top);
            _dockPanel.Children.Add(_title);

            _mainMenuPanel = new StackPanel();
            _dockPanel.Children.Add(_mainMenuPanel);

            _startButton = new SpriteButton();
            _mainMenuPanel.Children.Add(_startButton);

            _settingButton = new SpriteButton();
            _mainMenuPanel.Children.Add(_settingButton);

            _quitButton = new SpriteButton();
            _mainMenuPanel.Children.Add(_quitButton);
        }
    }
}
