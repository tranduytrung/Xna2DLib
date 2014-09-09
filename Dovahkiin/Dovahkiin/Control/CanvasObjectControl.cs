using Dovahkiin.Extension;
using Dovahkiin.Model.Action;
using Dovahkiin.Model.Core;
using tranduytrung.Xna.Core;

namespace Dovahkiin.Control
{
    public class CanvasObjectControl : ContentPresenter
    {
        public ICanvasObject Model
        {
            get { return this.GetCanvasObjectModel(); }
            set { this.SetCanvasObjectModel(value); }
        }

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

        public override void Update()
        {
            base.Update();

            SetValue(HybridMap.XProperty, Model.X);
            SetValue(HybridMap.YProperty, Model.Y);
        }

        public void MoveTo(int x, int y)
        {
            var obj = Model as Actor;
            if (obj == null) return;

            obj.DoAction(new MoveAction() { Target = obj, X = x, Y = y });
        }
    }
}