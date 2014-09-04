using Dovahkiin.Constant;
using Microsoft.Xna.Framework;
using tranduytrung.Xna.Control;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Engine;

namespace Dovahkiin.Screen
{
    public class StartupMenuScreen : ComponentBase
    {
        private Canvas _mainCanvas;
        private Sprite _background;

        private StackPanel _mainMenuPanel;
        //private Button _playButton;
        //private Button _settingButton;
        //private Button _quitButton;
        public StartupMenuScreen(Game game) : base(game)
        {
        }

        protected override void LoadContent()
        {
            _mainCanvas = new Canvas();
            PresentableContent = _mainCanvas;

            _background = new Sprite(new SingleSpriteSelector(Textures.MainMenuBackground));
            _background.SpriteMode = SpriteMode.Fit;
            _mainCanvas.Children.Add(_background);

            var buttonNormalTexture = Textures.ButtonNormal;
            var buttonHoverTexture = Textures.ButtonHover;
            var buttonPressedTexture = Textures.ButtonPressed;

            //_playButton = ControlFactory.CreateButton("play", buttonFont, buttonNormalTexture, buttonHoverTexture, buttonPressedTexture);
            //_playButton.SetValue(Panel.MarginProperty, new Margin(0, 12));
            //_playButton.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            //_playButton.Click += PlayNewGame;
            //_mainMenuPanel.Children.Add(_playButton);

            //_settingButton = ControlFactory.CreateButton("settings", buttonFont, buttonNormalTexture, buttonHoverTexture, buttonPressedTexture);
            //_settingButton.SetValue(Panel.MarginProperty, new Margin(0, 12));
            //_settingButton.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            //_mainMenuPanel.Children.Add(_settingButton);

            //_quitButton = ControlFactory.CreateButton("quit", buttonFont, buttonNormalTexture, buttonHoverTexture, buttonPressedTexture);
            //_quitButton.SetValue(Panel.MarginProperty, new Margin(0, 12));
            //_quitButton.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            //_quitButton.Click += QuitGame;
            //_mainMenuPanel.Children.Add(_quitButton);

            base.LoadContent();
        }
    }
}
