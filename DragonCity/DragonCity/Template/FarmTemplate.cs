using tranduytrung.DragonCity.Constant;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Map;

namespace tranduytrung.DragonCity.Template
{
    public class FarmTemplate : ITemplate
    {
        private IsometricMap _map;
        public DrawableObject PresentableContent { get; private set; }
        public DrawableObject ContextMenu { get; private set; }
        public void Start()
        {
        }

        public void End()
        {
        }

        public void ApplyData(IsometricMap map, object data)
        {
            _map = map;
        }

        public FarmTemplate()
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
            PresentableContent = new Sprite(new SingleSpriteSelector(Textures.Farm)) {SpriteMode = SpriteMode.Fit};
        }
    }
}