using tranduytrung.DragonCity.Constant;
using tranduytrung.Xna.Core;

namespace tranduytrung.DragonCity.Model
{
    public class InGameMenu : IService
    {
        public SpriteBase Logo { get; private set; }
        public DrawableObject ContextMenu { get; private set; }

        public InGameMenu()
        {
            Logo = new Sprite(new SingleSpriteSelector(Textures.Setting)) {SpriteMode = SpriteMode.Fit};
        }
    }
}
