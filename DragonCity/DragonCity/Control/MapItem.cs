using tranduytrung.Xna.Core;

namespace tranduytrung.DragonCity.Control
{
    public class MapItem : ContentPresenter
    {
        protected override void OnLeftMouseButtonDown(ref bool interupt)
        {
            base.OnLeftMouseButtonDown(ref interupt);
            interupt = true;
        }

        public override void OnLeftMouseButtonPressed(ref bool interupt)
        {
            base.OnLeftMouseButtonPressed(ref interupt);
            interupt = true;
        }

        protected override void OnLeftMouseButtonUp(ref bool interupt)
        {
            base.OnLeftMouseButtonUp(ref interupt);
            interupt = true;
        }

    }
}
