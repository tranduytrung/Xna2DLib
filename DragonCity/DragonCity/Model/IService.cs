using tranduytrung.Xna.Core;

namespace tranduytrung.DragonCity.Model
{
    public interface IService
    {
        SpriteBase Logo { get; }
        DrawableObject ContextMenu { get; }
        void Start();
        void End();
    }
}
