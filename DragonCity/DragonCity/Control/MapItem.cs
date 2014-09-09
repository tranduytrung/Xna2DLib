using Microsoft.Xna.Framework;
using tranduytrung.Xna.Core;

namespace tranduytrung.DragonCity.Control
{
    public class MapItem : ContentPresenter
    {
        protected override bool OnLeftMouseButtonDown(Vector2 relativePoint)
        {
            base.OnLeftMouseButtonDown(relativePoint);
            return false;
        }

        public override bool OnLeftMouseButtonPressed(Vector2 relativePoint)
        {
            base.OnLeftMouseButtonPressed(relativePoint);
            return false;
        }

        protected override bool OnLeftMouseButtonUp(Vector2 relativePoint)
        {
            base.OnLeftMouseButtonUp(relativePoint);
            return false;
        }

    }
}
