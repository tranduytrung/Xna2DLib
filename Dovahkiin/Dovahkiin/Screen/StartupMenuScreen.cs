
using Dovahkiin.Constant;
using Dovahkiin.Control;
using Dovahkiin.Repository;
using Dovahkiin.Utility;
using Microsoft.Xna.Framework;
using System;
using tranduytrung.Xna.Animation;
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
        private Sprite _title;

        private StackPanel _mainMenuPanel;
        private Button _playButton;
        private Button _resumeButton;
        private Button _settingButton;
        private Button _quitButton;
        public StartupMenuScreen(Game game) : base(game)
        {
            _mainMenuPanel = new StackPanel();
            
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

            _title = new Sprite(new SingleSpriteSelector(Resouces.GetTexture(Textures.MainMenuLogo)));
            _title.SpriteMode = SpriteMode.FitHorizontal;
            _title.TintingColor = Color.Transparent;
            _title.SetValue(Panel.MarginProperty, new Margin(0, 24, 48, 48));
            contentStack.Children.Add(_title);

            _mainMenuPanel.Orientation = StackOrientation.Vertical;
            _mainMenuPanel.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Right);
            contentStack.Children.Add(_mainMenuPanel);

            var titleAnimation =
                StoryBuilder.Select(_title)
                    .Wait(TimeSpan.FromMilliseconds(500))
                    .Animate("TintingColor", Color.Transparent, Color.White, TimeSpan.FromSeconds(1))
                    .ToStoryboard();
            AnimationManager.BeginAnimation(titleAnimation);

            base.LoadContent();
        }

        public override void OnTransitFrom()
        {
            base.OnTransitFrom();

            _mainMenuPanel.Children.Clear();
            var buttonNormalTexture = Resouces.GetTexture(Textures.ButtonNormal);
            var buttonHoverTexture = Resouces.GetTexture(Textures.ButtonHover);
            var buttonPressedTexture = Resouces.GetTexture(Textures.ButtonPressed);
            var buttonFont = Resouces.GetFont(Fonts.ButtonFont);

            _playButton = ControlFactory.CreateButton("New Game");
            _playButton.SetValue(Panel.MarginProperty, new Margin(50, 12));
            _playButton.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _playButton.Click += PlayNewGame;
            _mainMenuPanel.Children.Add(_playButton);

            if (GlobalConfig.GameStarted)
            {
                _resumeButton = ControlFactory.CreateButton("Resume");
                _resumeButton.SetValue(Panel.MarginProperty, new Margin(50, 12));
                _resumeButton.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                _resumeButton.Click += ResumeGame;
                _mainMenuPanel.Children.Add(_resumeButton);
            }

            _settingButton = ControlFactory.CreateButton("settings");
            _settingButton.SetValue(Panel.MarginProperty, new Margin(0, 12));
            _settingButton.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _settingButton.Click += OnBtnSettingClick;
            _mainMenuPanel.Children.Add(_settingButton);

            _quitButton = ControlFactory.CreateButton("quit");
            _quitButton.SetValue(Panel.MarginProperty, new Margin(0, 12));
            _quitButton.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _quitButton.Click += QuitGame;
            _mainMenuPanel.Children.Add(_quitButton);
        }

        private void ResumeGame(object sender, MouseEventArgs e)
        {
            GameContext.GameInstance.ChangeScreen(Dovahkiin.GamePlayScreen);
        }
        private void QuitGame(object sender, MouseEventArgs e)
        {
            Game.Exit();
        }

        private void PlayNewGame(object sender, MouseEventArgs e)
        {
            if (!GlobalConfig.GameStarted)
            {
                GlobalConfig.GameStarted = true;
            }
            ((Dovahkiin)GameContext.GameInstance).NewGamePlayScreen();
            GameContext.GameInstance.ChangeScreen(Dovahkiin.GamePlayScreen);
        }
        private void OnBtnSettingClick(object sender, MouseEventArgs e)
        {
            GameContext.GameInstance.ChangeScreen(Dovahkiin.SettingScreen);
        }
    }
}
