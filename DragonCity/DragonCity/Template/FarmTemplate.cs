using System;
using tranduytrung.DragonCity.Constant;
using tranduytrung.DragonCity.Model;
using tranduytrung.DragonCity.Utility;
using tranduytrung.Xna.Control;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Map;

namespace tranduytrung.DragonCity.Template
{
    public class FarmTemplate : ITemplate
    {
        private Farm _model;
        private readonly Timer _timerText;
        private Timer _timerGeneration;
        private SpriteText _generationText;

        public DrawableObject PresentableContent { get; private set; }
        public DrawableObject ContextMenu { get; private set; }
        

        public FarmTemplate()
        {
            SetupPresentableContent();
            _timerText = new Timer();
            _timerText.Callback += UpdateText;
            _timerText.Internal = TimeSpan.FromSeconds(1);
        }


        public void Start()
        {
            UpdateText(null, null);
            _timerText.Start();
        }

        public void End()
        {
            _timerText.End();
        }

        public void ApplyData(object data)
        {
            _model = (Farm)data;
            ((ContentPresenter)PresentableContent).Click += OnSelected;
            SetupContextMenu();
            SetupFoodGeneration();
        }

        private void SetupFoodGeneration()
        {
            if (_timerGeneration != null)
                _timerGeneration.End();

            _timerGeneration = new Timer {Internal = _model.GenerationTime};
            _timerGeneration.Callback += GenerateFood;
            _timerGeneration.Start();
        }

        void GenerateFood(object sender, EventArgs e)
        {
            DragonCity.GamePlay.AddFoods(_model.FoodGeneration);
        }

        private void UpdateText(object sender, EventArgs e)
        {
            var remainTime = (_model.GenerationTime - _timerGeneration.AccumulatedTime).TotalSeconds;
            _generationText.Text = string.Format("Produces {0} foods in {1}", _model.FoodGeneration, (int)remainTime);
        }

        private static void OnSelected(object sender, MouseEventArgs e)
        {
            DragonCity.GamePlay.Select((DrawableObject)sender);
        }

        private void SetupContextMenu()
        {
            var mainStack = new StackPanel {Orientation = StackOrientation.Vertical};

            var title = new SpriteText(Fonts.ButtonFont) {Text = "Farm"};
            title.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            mainStack.Children.Add(title);

            _generationText = new SpriteText(Fonts.ButtonFont);
            mainStack.Children.Add(_generationText);

            var sellStack = new StackPanel();
            mainStack.Children.Add(sellStack);

            var sellButton = ControlFactory.CreateSmallButton("sell", Fonts.ButtonFont, Textures.ButtonNormal,
                Textures.ButtonHover, Textures.ButtonPressed);
            sellButton.Click += Sell;
            sellStack.Children.Add(sellButton);

            var sellValueText = new SpriteText(Fonts.ButtonFont);
            sellValueText.Text = string.Format(" for {0} golds", _model.SellValue);
            sellStack.Children.Add(sellValueText);

            ContextMenu = mainStack;
        }

        private void SetupPresentableContent()
        {
            var sprite = new Sprite(new SingleSpriteSelector(Textures.Farm)) {SpriteMode = SpriteMode.FitHorizontal};
            var container = new ContentPresenter {PresentableContent = sprite};
            container.SetValue(IsometricMap.DeploymentProperty, new FourDiamondsDeployment());
            PresentableContent = container;
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
    }
}