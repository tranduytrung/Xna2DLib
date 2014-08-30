using System;
using tranduytrung.DragonCity.Constant;
using tranduytrung.DragonCity.Model;
using tranduytrung.DragonCity.Utility;
using tranduytrung.Xna.Control;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Map;

namespace tranduytrung.DragonCity.Template
{
    public class DragonTemplate : ITemplate
    {
        private Dragon _model;
        private readonly Timer _timerText;
        private Timer _timerGeneration;
        private SpriteText _generationText;
        private SpriteText _title;
        private SpriteText _foodText;

        public DrawableObject PresentableContent { get; private set; }
        public DrawableObject ContextMenu { get; private set; }

        public DragonTemplate()
        {
            SetupPresentableContent();
            _timerText = new Timer();
            _timerText.Callback += UpdateTimeText;
            _timerText.Internal = TimeSpan.FromSeconds(1);
        }

        public void Start()
        {
            UpdateTimeText(null, null);
            _timerText.Start();
        }

        public void End()
        {
            _timerText.End();
        }

        public void ApplyData(object data)
        {
            _model = (Dragon)data;
            ((ContentPresenter)PresentableContent).Click += OnSelected;
            SetupContextMenu();
            SetupGoldGeneration();
        }

        private void SetupGoldGeneration()
        {
            if (_timerGeneration != null)
            {
                _timerGeneration.End();
                _timerGeneration.Callback -= GenerateGold;
            }
            
            _timerGeneration = new Timer { Internal = _model.GenerationTime };
            _timerGeneration.Callback += GenerateGold;
            _timerGeneration.Start();
        }

        void GenerateGold(object sender, EventArgs e)
        {
            DragonCity.GamePlay.AddGolds(_model.GoldGeneration);
        }

        private void UpdateTimeText(object sender, EventArgs e)
        {
            var remainTime = (_model.GenerationTime - _timerGeneration.AccumulatedTime).TotalSeconds;
            _generationText.Text = string.Format("Produces {0} golds in {1}", _model.GoldGeneration, (int)remainTime);
        }

        private static void OnSelected(object sender, MouseEventArgs e)
        {
            DragonCity.GamePlay.Select((DrawableObject)sender);
        }


        private void SetupContextMenu()
        {
            #region panel

            var mainStack = new StackPanel { Orientation = StackOrientation.Vertical };
            ContextMenu = mainStack;

            #endregion

            #region title

            _title = new SpriteText(Fonts.ButtonFont);
            _title.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            mainStack.Children.Add(_title);
            
            #endregion

            #region generation

            _generationText = new SpriteText(Fonts.ButtonFont);
            mainStack.Children.Add(_generationText);

            #endregion

            #region feeding

            var feedStack = new StackPanel();
            mainStack.Children.Add(feedStack);

            _foodText = new SpriteText(Fonts.ButtonFont);
            feedStack.Children.Add(_foodText);

            var foodIcon = new Sprite(new SingleSpriteSelector(Textures.Tomato))
            {
                Width = ControlConfig.ResouceIconWidth,
                Height = ControlConfig.ResouceIconHeight,
                SpriteMode = SpriteMode.Fit,
                Margin = new Margin(6, 0)
            };
            feedStack.Children.Add(foodIcon);

            var feedInfoText = new SpriteText(Fonts.ButtonFont);
            feedInfoText.Text = "to level up";
            feedStack.Children.Add(feedInfoText);

            var feedButton = ControlFactory.CreateSmallButton("feed", Fonts.ButtonFont, Textures.ButtonNormal,
                Textures.ButtonHover, Textures.ButtonPressed);
            feedButton.Margin = new Margin(6, 0);
            feedButton.Click +=Feed;
            feedStack.Children.Add(feedButton);

            UpdateLevelText();

            #endregion

            #region sell

            var sellStack = new StackPanel();
            mainStack.Children.Add(sellStack);
            
            var sellButton = ControlFactory.CreateSmallButton("sell", Fonts.ButtonFont, Textures.ButtonNormal,
                Textures.ButtonHover, Textures.ButtonPressed);
            sellButton.Click += Sell;
            sellStack.Children.Add(sellButton);

            var sellValueText = new SpriteText(Fonts.ButtonFont);
            sellValueText.Text = string.Format(" for {0} golds",_model.SellValue);
            sellStack.Children.Add(sellValueText);

            #endregion
        }

        private void Feed(object sender, MouseEventArgs e)
        {
            var game = DragonCity.GamePlay;
            var foodNeed = _model.MaxFoodGauge - _model.AccumulatedFood;
            if (game.ConsumeFoods(foodNeed))
            {
                _model.Feed(foodNeed);
                SetupGoldGeneration();
                UpdateLevelText();
            }
            else
            {
                _model.Feed(game.Foods);
                game.ConsumeFoods(game.Foods);
                UpdateLevelText();
            }
        }

        private void UpdateLevelText()
        {
            _title.Text = string.Format("{0} - level {1}", _model.Name, _model.Level);
            _foodText.Text = string.Format("needs {0}", _model.MaxFoodGauge - _model.AccumulatedFood);
        }

        private void Sell(object sender, MouseEventArgs e)
        {
            DragonCity.GamePlay.AddGolds(_model.SellValue);
            Destroy();
        }

        private void Destroy()
        {
            DragonCity.GamePlay.MapControl.RemoveChild(PresentableContent);
            DragonCity.GamePlay.Unselect();
            _timerGeneration.End();
            _timerText.End();
        }

        private void SetupPresentableContent()
        {
            var sprite = new Sprite(new SingleSpriteSelector(Textures.Poo)) {SpriteMode = SpriteMode.FitHorizontal};
            var container = new ContentPresenter {PresentableContent = sprite};
            container.SetValue(IsometricMap.DeploymentProperty, new UnitDeployment());
            PresentableContent = container;
        }

    }
}