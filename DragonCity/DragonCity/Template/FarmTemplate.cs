using tranduytrung.DragonCity.Constant;
using tranduytrung.DragonCity.Model;
using tranduytrung.Xna.Control;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Map;

namespace tranduytrung.DragonCity.Template
{
    public class FarmTemplate : ITemplate
    {
        private Farm _model;
        public DrawableObject PresentableContent { get; private set; }
        public DrawableObject ContextMenu { get; private set; }

        public void Start()
        {
        }

        public void End()
        {
        }

        public void ApplyData(object data)
        {
            _model = (Farm)data;
            ((ContentPresenter)PresentableContent).Click += OnSelected;
            SetupContextMenu();
        }

        private void OnSelected(object sender, MouseEventArgs e)
        {
            DragonCity.GamePlay.Select((DrawableObject)sender);
        }

        public FarmTemplate()
        {
            SetupPresentableContent();
        }

        private void SetupContextMenu()
        {
            var mainStack = new StackPanel();
            mainStack.Orientation = StackOrientation.Vertical;

            var title = new SpriteText(Fonts.ButtonFont);
            title.Text = "Farm";
            title.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            mainStack.Children.Add(title);

            ContextMenu = mainStack;
        }

        private void SetupPresentableContent()
        {
            var sprite = new Sprite(new SingleSpriteSelector(Textures.Farm)) {SpriteMode = SpriteMode.FitHorizontal};
            var container = new ContentPresenter();
            container.PresentableContent = sprite;
            container.SetValue(IsometricMap.DeploymentProperty, new FourDiamondsDeployment());
            PresentableContent = container;
        }
    }
}