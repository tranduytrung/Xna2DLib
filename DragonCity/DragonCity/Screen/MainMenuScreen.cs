﻿using System;
using Microsoft.Xna.Framework;
using tranduytrung.DragonCity.Constant;
using tranduytrung.DragonCity.Control;
using tranduytrung.DragonCity.Utility;
using tranduytrung.Xna.Animation;
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
        private Button _playButton;
        private Button _settingButton;
        private Button _quitButton;
        private Storyboard _mainMenuAnimation;

        public MainMenuScreen(Game game) : base(game)
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
            contentStack.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Top);
            _dockPanel.Children.Add(contentStack);

            _title = new Sprite(new SingleSpriteSelector(Textures.MainMenuLogo));
            _title.SpriteMode = SpriteMode.FitHorizontal;
            _title.TintingColor = Color.Transparent;
            _title.SetValue(Panel.MarginProperty, new Margin(0, 24, 48, 48));
            contentStack.Children.Add(_title);

            _mainMenuPanel = new StackPanel();
            _mainMenuPanel.Orientation = StackOrientation.Vertical;
            _mainMenuPanel.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentStack.Children.Add(_mainMenuPanel);

            var buttonNormalTexture = Textures.ButtonNormal;
            var buttonHoverTexture = Textures.ButtonHover;
            var buttonPressedTexture = Textures.ButtonPressed;
            var buttonFont = Fonts.ButtonFont;

            _playButton = ControlFactory.CreateButton("play", buttonFont, buttonNormalTexture, buttonHoverTexture, buttonPressedTexture);
            _playButton.SetValue(Panel.MarginProperty, new Margin(0, 12));
            _playButton.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _playButton.Click += PlayNewGame;
            _mainMenuPanel.Children.Add(_playButton);

            _settingButton = ControlFactory.CreateButton("settings", buttonFont, buttonNormalTexture, buttonHoverTexture, buttonPressedTexture);
            _settingButton.SetValue(Panel.MarginProperty, new Margin(0, 12));
            _settingButton.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _mainMenuPanel.Children.Add(_settingButton);

            _quitButton = ControlFactory.CreateButton("quit", buttonFont, buttonNormalTexture, buttonHoverTexture, buttonPressedTexture);
            _quitButton.SetValue(Panel.MarginProperty, new Margin(0, 12));
            _quitButton.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _quitButton.Click += QuitGame;
            _mainMenuPanel.Children.Add(_quitButton);


            var titleAnimation =
                StoryBuilder.Select(_title)
                    .Wait(TimeSpan.FromMilliseconds(500))
                    .Animate("TintingColor", Color.Transparent, Color.White, TimeSpan.FromSeconds(1))
                    .ToStoryboard();
            AnimationManager.BeginAnimation(titleAnimation);

            _mainMenuAnimation =
                StoryBuilder.Select(_playButton, _settingButton, _quitButton)
                    .Animate("Margin.Left", 512, 0, TimeSpan.FromMilliseconds(300))
                    .Remove(_playButton)
                    .Animate("Margin.Right", 48, TimeSpan.FromMilliseconds(100))
                    .Remove(_settingButton)
                    .Animate("Margin.Right", 96, TimeSpan.FromMilliseconds(100))
                    .Join(StoryBuilder.Select(_playButton).Animate("TintingColor", Color.Transparent, Color.White, TimeSpan.FromMilliseconds(500)))
                    .Join(StoryBuilder.Select(_settingButton).Animate("TintingColor", Color.Transparent, Color.White, TimeSpan.FromMilliseconds(600)))
                    .Join(StoryBuilder.Select(_quitButton).Animate("TintingColor", Color.Transparent, Color.White, TimeSpan.FromMilliseconds(700)))
                    .ToStoryboard();

            AnimationManager.BeginAnimation(_mainMenuAnimation);
        }

        private void QuitGame(object sender, MouseEventArgs e)
        {
            Game.Exit();
        }

        private void PlayNewGame(object sender, MouseEventArgs e)
        {
            GameContext.GameInstance.ChangeScreen(DragonCity.GamePlay);
        }
    }
}
