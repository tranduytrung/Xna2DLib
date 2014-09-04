using Dovahkiin.Constant;
using Dovahkiin.Control;
using Dovahkiin.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            _background = new Sprite(new SingleSpriteSelector(Textures.MainMenuBackground));
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

            var buttonNormalTexture = Textures.ButtonNormal;
            var buttonHoverTexture = Textures.ButtonHover;
            var buttonPressedTexture = Textures.ButtonPressed;
            var buttonFont = Fonts.ButtonFont;

            string soundStr = null;
            if (GlobalConfig.SoundEnabled)
                soundStr = "sound: on";
            else
                soundStr = "sound: off";

            _soundButton = ControlFactory.CreateButton(soundStr, buttonFont, buttonNormalTexture, buttonHoverTexture, buttonPressedTexture);
            _soundButton.SetValue(Panel.MarginProperty, new Margin(0, 12));
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
                buttonText = new SpriteText(Fonts.ButtonFont) { Text = "sound: off" };
            }
            else
            {
                GlobalConfig.SoundEnabled = true;
                buttonText = new SpriteText(Fonts.ButtonFont) { Text = "sound: on" };
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
                buttonText = new SpriteText(Fonts.ButtonFont) { Text = "music: off" };
            }
            else
            {
                GlobalConfig.SoundEnabled = true;
                buttonText = new SpriteText(Fonts.ButtonFont) { Text = "music: on" };
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
