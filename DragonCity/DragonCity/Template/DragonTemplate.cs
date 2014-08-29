using tranduytrung.DragonCity.Constant;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Map;

namespace tranduytrung.DragonCity.Template
{
    public class DragonTemplate : ITemplate
    {
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
        }

        public DragonTemplate()
        {
            SetupPresentableContent();
            SetupContextMenu();
        }

        private void SetupContextMenu()
        {
            ContextMenu = null;
        }

        private void SetupPresentableContent()
        {
            var sprite = new Sprite(new SingleSpriteSelector(Textures.Poo)) {SpriteMode = SpriteMode.FitHorizontal};
            sprite.SetValue(IsometricMap.DeploymentProperty, new UnitDeployment());
            PresentableContent = sprite;
        }

    }
}