using System;
using tranduytrung.DragonCity.Constant;
using tranduytrung.DragonCity.Control;
using tranduytrung.DragonCity.Model;
using tranduytrung.DragonCity.Utility;
using tranduytrung.Xna.Control;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Map;

namespace tranduytrung.DragonCity.Template
{
    public class HabitatTemplate : ITemplate
    {
        private Habitat _model;
        private DragonTemplate _dragon;

        public DrawableObject PresentableContent { get; private set; }
        public DrawableObject ContextMenu { get; private set; }

        public HabitatTemplate()
        {
            SetupPresentableContent();
        }

        public virtual bool CanDeploy(Type type)
        {
            return typeof (DragonTemplate) == type && _dragon == null;
        }

        public void Start()
        {
        }

        public void End()
        {
        }

        public void ApplyData(object data)
        {
            _model = (Habitat)data;
            ((ContentPresenter)PresentableContent).Click += OnSelected;
            SetupContextMenu();
        }

        private static void OnSelected(object sender, MouseEventArgs e)
        {
            DragonCity.GamePlay.Select((DrawableObject)sender);
        }


        private void SetupPresentableContent()
        {
            var sprite = new Sprite(new SingleSpriteSelector(Textures.Terra)) { SpriteMode = SpriteMode.FitHorizontal };
            var container = new MapItem { PresentableContent = sprite };
            container.SetValue(IsometricMap.DeploymentProperty, new FourDiamondsDeployment());
            PresentableContent = container;
        }

        private void SetupContextMenu()
        {
            var mainStack = new StackPanel { Orientation = StackOrientation.Vertical };

            var title = new SpriteText(Fonts.ButtonFont) { Text = "Habitat" };
            title.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            mainStack.Children.Add(title);

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

        private void Sell(object sender, MouseEventArgs e)
        {
            if (_dragon != null)
                return;

            DragonCity.GamePlay.AddGolds(_model.SellValue);
            Destroy();
        }

        private void Destroy()
        {
            DragonCity.GamePlay.MapControl.RemoveChild(PresentableContent);
            DragonCity.GamePlay.Unselect();
        }

        public void Deploy(ITemplate dragon)
        {
            if (!CanDeploy(dragon.GetType()))
                return;
            _dragon = (DragonTemplate)dragon;
            _dragon.SetHabitat(this);
        }

        public void Undeploy()
        {
            _dragon = null;
        }
    }
}