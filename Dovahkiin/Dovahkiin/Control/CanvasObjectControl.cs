using Dovahkiin.Extension;
using Dovahkiin.Model.Action;
using Dovahkiin.Model.Core;
using Dovahkiin.Repository;
using Microsoft.Xna.Framework;
using System;
using tranduytrung.Xna.Core;

namespace Dovahkiin.Control
{
    public class CanvasObjectControl : ContentPresenter
    {
        public CanvasObjectControl(int resourceId, ICanvasObject model)
        {
            _resourceId = resourceId;
            PresentableContent = new Sprite(new ComplexMultipleSpriteSelector(Resouces.GetComplexTexture(resourceId), State.walking, Direction.e));
            Model = model;

            SetValue(HybridMap.XProperty, model.X);
            SetValue(HybridMap.YProperty, model.Y);
        }
        private int _resourceId;
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
            int dX = x - (int)this.GetValue(HybridMap.XProperty);
            int dY = y - (int)this.GetValue(HybridMap.YProperty);
            Direction newDirection = Direction.e;
            if (dX == 0)
            {
                if (dY < 0)
                    newDirection = Direction.n;
                else
                    newDirection = Direction.s;
            }
            else if (dY == 0)
            {
                if (dX > 0)
                    newDirection = Direction.e;
                else
                    newDirection = Direction.w;
            }
            else
            {
                double tan = (double)dY / (double)dX;
                if (tan > -Math.Sqrt(3) / 3 && tan < Math.Sqrt(3) / 3)
                {
                    if (dX > 0)
                        newDirection = Direction.e;
                    else
                        newDirection = Direction.w;
                }
                else if (tan > Math.Sqrt(3) / 3 && tan < Math.Sqrt(3))
                {
                    if (dX > 0)
                        newDirection = Direction.se;
                    else
                        newDirection = Direction.nw;
                }
                else if (tan > Math.Sqrt(3) || tan < -Math.Sqrt(3))
                {
                    if (dY < 0)
                        newDirection = Direction.n;
                    else
                        newDirection = Direction.s;
                }
                else if (tan < -Math.Sqrt(3) / 3 && tan > -Math.Sqrt(3))
                {
                    if (dX > 0)
                        newDirection = Direction.ne;
                    else
                        newDirection = Direction.sw;
                }
            }

            PresentableContent = new Sprite(new ComplexMultipleSpriteSelector(Resouces.GetComplexTexture(_resourceId), State.walking, newDirection));
            var obj = Model as Actor;
            if (obj == null) return;

            obj.DoAction(new Move() { X = x, Y = y });
        }
    }
}