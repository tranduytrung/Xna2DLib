using System;
using System.Diagnostics;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

            _background = new Sprite(new SingleSpriteSelector(Game.Content.Load<Texture2D>(@"images/menu/background")));
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

            _title = new Sprite(new SingleSpriteSelector(Game.Content.Load<Texture2D>(@"images/menu/logo")));
            _title.SpriteMode = SpriteMode.FitHorizontal;
            _title.TintingColor = Color.Transparent;
            _title.SetValue(Panel.MarginProperty, new Margin(0, 24, 48, 48));
            contentStack.Children.Add(_title);

            _mainMenuPanel = new StackPanel();
            _mainMenuPanel.Orientation = StackOrientation.Vertical;
            _mainMenuPanel.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentStack.Children.Add(_mainMenuPanel);

            var buttonTexture = Game.Content.Load<Texture2D>(@"images/menu/button");
            var buttonFont = Game.Content.Load<SpriteFont>(@"fonts/button_font");

            _startButton = ControlFactory.CreateButton(buttonTexture, buttonFont, "start");
            _startButton.SetValue(Panel.MarginProperty, new Margin(0, 12));
            _startButton.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _startButton.Width = 220;
            _startButton.Height = 72;
            _mainMenuPanel.Children.Add(_startButton);

            _settingButton = ControlFactory.CreateButton(buttonTexture, buttonFont, "setting");
            _settingButton.SetValue(Panel.MarginProperty, new Margin(0, 12, 48, 12));
            _settingButton.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _settingButton.Width = 220;
            _settingButton.Height = 72;
            _mainMenuPanel.Children.Add(_settingButton);

            _quitButton = ControlFactory.CreateButton(buttonTexture, buttonFont, "quit");
            _quitButton.SetValue(Panel.MarginProperty, new Margin(0, 12, 96, 12));
            _quitButton.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _quitButton.Width = 220;
            _quitButton.Height = 72;
            _mainMenuPanel.Children.Add(_quitButton);


            var titleAnimation =
                StoryBuilder.Select(_title)
                    .Wait(TimeSpan.FromMilliseconds(500))
                    .Animate("TintingColor", Color.Transparent, Color.White, TimeSpan.FromSeconds(2))
                    .ToStoryboard();
            AnimationManager.BeginAnimation(titleAnimation);
        }
    }
}
