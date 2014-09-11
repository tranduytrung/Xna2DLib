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
    public class SettingScreen : ComponentBase
    {
        private Canvas _mainCanvas;
        private DockPanel _dockPanel;
        private Sprite _background;
        private Sprite _title;

        private StackPanel _mainMenuPanel;
        private Button _soundButton;
        private Button _musicButton;
        private Button _backButton;

        public SettingScreen(Game game)
            : base(game)
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
            contentStack.Width = 715;
            contentStack.Orientation = StackOrientation.Vertical;
            contentStack.SetValue(DockPanel.DockProperty, Dock.Right);
            contentStack.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Bottom);
            _dockPanel.Children.Add(contentStack);

            _title = new Sprite(new SingleSpriteSelector(Resouces.GetTexture(Textures.SettingMenuLogo)));
            _title.SpriteMode = SpriteMode.FitHorizontal;
            _title.SetValue(Panel.MarginProperty, new Margin(0, 24, 48, 48));
            contentStack.Children.Add(_title);

            _mainMenuPanel = new StackPanel();
            _mainMenuPanel.Orientation = StackOrientation.Vertical;
            _mainMenuPanel.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Right);
            contentStack.Children.Add(_mainMenuPanel);

            var buttonNormalTexture = Resouces.GetTexture(Textures.ButtonNormal);
            var buttonHoverTexture = Resouces.GetTexture(Textures.ButtonHover);
            var buttonPressedTexture = Resouces.GetTexture(Textures.ButtonPressed);
            var buttonFont = Resouces.GetFont(Fonts.ButtonFont);

            string soundStr;
            soundStr = GlobalConfig.SoundEnabled ? "sound: on" : "sound: off";

            _soundButton = ControlFactory.CreateButton(soundStr, buttonFont, buttonNormalTexture, buttonHoverTexture, buttonPressedTexture);
            _soundButton.SetValue(Panel.MarginProperty, new Margin(50, 12));
            _soundButton.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _soundButton.Click += OnBtnSoundClick;
            _mainMenuPanel.Children.Add(_soundButton);

            string musicStr = null;
            if (GlobalConfig.MusicEnabled)
                musicStr = "music: on";
            else
                musicStr = "music: off";
            _musicButton = ControlFactory.CreateButton(musicStr, buttonFont, buttonNormalTexture, buttonHoverTexture, buttonPressedTexture);
            _musicButton.SetValue(Panel.MarginProperty, new Margin(0, 12));
            _musicButton.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _musicButton.Click += OnBtnMusicClick;
            _mainMenuPanel.Children.Add(_musicButton);

            _backButton = ControlFactory.CreateButton("back", buttonFont, buttonNormalTexture, buttonHoverTexture, buttonPressedTexture);
            _backButton.SetValue(Panel.MarginProperty, new Margin(0, 12));
            _backButton.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _backButton.Click += OnBtnBackClick;
            _mainMenuPanel.Children.Add(_backButton);

            base.LoadContent();
        }

        private void OnBtnSoundClick(object sender, MouseEventArgs e)
        {
            var buttonText = _soundButton.PresentableContent;
            HorizontalAlignment hAlign = (HorizontalAlignment)buttonText.GetValue(AlignmentExtension.HorizontalAlignmentProperty);
            VerticalAlignment vAlign = (VerticalAlignment)buttonText.GetValue(AlignmentExtension.VerticalAlignmentProperty);
            if (GlobalConfig.SoundEnabled)
            {
                GlobalConfig.SoundEnabled = false;
                buttonText = new SpriteText(Resouces.GetFont(Fonts.ButtonFont)) { Text = "sound: off" };
            }
            else
            {
                GlobalConfig.SoundEnabled = true;
                buttonText = new SpriteText(Resouces.GetFont(Fonts.ButtonFont)) { Text = "sound: on" };
            }
            buttonText.SetValue(AlignmentExtension.HorizontalAlignmentProperty, hAlign);
            buttonText.SetValue(AlignmentExtension.VerticalAlignmentProperty, vAlign);

            _soundButton.PresentableContent = buttonText;
        }
        private void OnBtnMusicClick(object sender, MouseEventArgs e)
        {
            var buttonText = _soundButton.PresentableContent;
            HorizontalAlignment hAlign = (HorizontalAlignment)buttonText.GetValue(AlignmentExtension.HorizontalAlignmentProperty);
            VerticalAlignment vAlign = (VerticalAlignment)buttonText.GetValue(AlignmentExtension.VerticalAlignmentProperty);
            if (GlobalConfig.SoundEnabled)
            {
                GlobalConfig.SoundEnabled = false;
                buttonText = new SpriteText(Resouces.GetFont(Fonts.ButtonFont)) { Text = "music: off" };
            }
            else
            {
                GlobalConfig.SoundEnabled = true;
                buttonText = new SpriteText(Resouces.GetFont(Fonts.ButtonFont)) { Text = "music: on" };
            }
            buttonText.SetValue(AlignmentExtension.HorizontalAlignmentProperty, hAlign);
            buttonText.SetValue(AlignmentExtension.VerticalAlignmentProperty, vAlign);

            _musicButton.PresentableContent = buttonText;
        }
        private void OnBtnBackClick(object sender, MouseEventArgs e)
        {
            GameContext.GameInstance.ChangeScreen(Dovahkiin.StartupMenuScreen);
        }
    }
}
