
using Dovahkiin.Constant;
using Dovahkiin.Control;
using Dovahkiin.Repository;
using Dovahkiin.Utility;
using Microsoft.Xna.Framework;
using tranduytrung.Xna.Control;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Engine;
namespace Dovahkiin.Screen
{
    public class StartupMenuScreen : ComponentBase
    {
        private Canvas _mainCanvas;
        private DockPanel _dockPanel;
        private Sprite _background;

        private StackPanel _mainMenuPanel;
        private Button _playButton;
        private Button _settingButton;
        private Button _quitButton;
        public StartupMenuScreen(Game game) : base(game)
        {
        }

        protected override void LoadContent()
        {
            _mainCanvas = new Canvas();
            PresentableContent = _mainCanvas;

            _background = new Sprite(new SingleSpriteSelector(Resouces.GetTexture(Textures.MainMenuBackground)));
            _background.SpriteMode = SpriteMode.Fit;
            _mainCanvas.Children.Add(_background);

            _dockPanel = new DockPanel();
            _dockPanel.AutoFillLastChild = false;
            _mainCanvas.Children.Add(_dockPanel);

            var contentStack = new StackPanel();
            contentStack.Width = 512;
            contentStack.Orientation = StackOrientation.Vertical;
            contentStack.SetValue(DockPanel.DockProperty, Dock.Right);
            contentStack.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Bottom);
            _dockPanel.Children.Add(contentStack);

            _mainMenuPanel = new StackPanel();
            _mainMenuPanel.Orientation = StackOrientation.Vertical;
            _mainMenuPanel.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentStack.Children.Add(_mainMenuPanel);

            var buttonNormalTexture = Resouces.GetTexture(Textures.ButtonNormal);
            var buttonHoverTexture = Resouces.GetTexture(Textures.ButtonHover);
            var buttonPressedTexture = Resouces.GetTexture(Textures.ButtonPressed);
            var buttonFont = Resouces.GetFont(Fonts.ButtonFont);

            _playButton = ControlFactory.CreateButton("play", buttonFont, buttonNormalTexture, buttonHoverTexture, buttonPressedTexture);
            _playButton.SetValue(Panel.MarginProperty, new Margin(0, 12));
            _playButton.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _playButton.Click += PlayNewGame;
            _mainMenuPanel.Children.Add(_playButton);

            _settingButton = ControlFactory.CreateButton("settings", buttonFont, buttonNormalTexture, buttonHoverTexture, buttonPressedTexture);
            _settingButton.SetValue(Panel.MarginProperty, new Margin(0, 12));
            _settingButton.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _settingButton.Click += OnBtnSettingClick;
            _mainMenuPanel.Children.Add(_settingButton);

            _quitButton = ControlFactory.CreateButton("quit", buttonFont, buttonNormalTexture, buttonHoverTexture, buttonPressedTexture);
            _quitButton.SetValue(Panel.MarginProperty, new Margin(0, 12));
            _quitButton.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _quitButton.Click += QuitGame;
            _mainMenuPanel.Children.Add(_quitButton);

            base.LoadContent();
        }
        private void QuitGame(object sender, MouseEventArgs e)
        {
            Game.Exit();
        }

        private void PlayNewGame(object sender, MouseEventArgs e)
        {
            GameContext.GameInstance.ChangeScreen(Dovahkiin.GamePlayScreen);
        }
        private void OnBtnSettingClick(object sender, MouseEventArgs e)
        {
            GameContext.GameInstance.ChangeScreen(Dovahkiin.SettingScreen);
        }
    }
}
