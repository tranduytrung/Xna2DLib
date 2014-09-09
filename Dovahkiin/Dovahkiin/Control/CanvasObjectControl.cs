using Dovahkiin.Extension;
using Dovahkiin.Model.Action;
using Dovahkiin.Model.Core;
using Microsoft.Xna.Framework;
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