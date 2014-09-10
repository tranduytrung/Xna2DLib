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
        private readonly ComplexMultipleSpriteSelector _selector;
        private Timer _updateTimer;

        public CanvasObjectControl(ICanvasObject model)
        {
            _selector = new ComplexMultipleSpriteSelector(Resouces.GetComplexTexture(model.ResouceId), State.stopped, Direction.e);

            PresentableContent = new Sprite(_selector);
            Model = model;
            SetValue(HybridMap.XProperty, model.X);
            SetValue(HybridMap.YProperty, model.Y);
            _updateTimer = new Timer();
            _updateTimer.Internal = TimeSpan.FromSeconds(0.5);
            _updateTimer.Callback += OnUpdate;
            _updateTimer.Start();
        }

        private void OnUpdate(object sender, EventArgs e)
        {
            UpdateSelector();
        }

        public ICanvasObject Model
        {
            get { return this.GetCanvasObjectModel(); }
            private set { this.SetCanvasObjectModel(value); }
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

            if ((int)GetValue(HybridMap.XProperty) != Model.X || (int)GetValue(HybridMap.YProperty) != Model.Y)
                _selector.State = State.walking;
            else
                _selector.State = State.stopped;

            SetValue(HybridMap.XProperty, Model.X);
            SetValue(HybridMap.YProperty, Model.Y);
        }

        public void MoveTo(int x, int y)
        {
            var obj = Model as Actor;
            if (obj == null) return;

            obj.DoAction(new Move() { X = x, Y = y });
        }

        private void UpdateSelector()
        {
            var dX = Model.X - (int)GetValue(HybridMap.XProperty);
            var dY = Model.Y - (int)GetValue(HybridMap.YProperty);

            if (dX == 0 && dY == 0)
                return;

            var newDirection = Direction.e;
            if (dX == 0)
            {
                newDirection = dY < 0 ? Direction.n : Direction.s;
            }
            else if (dY == 0)
            {
                newDirection = dX > 0 ? Direction.e : Direction.w;
            }
            else
            {
                var tan = (double)dY / dX;
                if (tan > -Math.Sqrt(3) / 3 && tan < Math.Sqrt(3) / 3)
                {
                    newDirection = dX > 0 ? Direction.e : Direction.w;
                }
                else if (tan > Math.Sqrt(3) / 3 && tan < Math.Sqrt(3))
                {
                    newDirection = dX > 0 ? Direction.se : Direction.nw;
                }
                else if (tan > Math.Sqrt(3) || tan < -Math.Sqrt(3))
                {
                    newDirection = dY < 0 ? Direction.n : Direction.s;
                }
                else if (tan < -Math.Sqrt(3) / 3 && tan > -Math.Sqrt(3))
                {
                    newDirection = dX > 0 ? Direction.ne : Direction.sw;
                }
            }

            _selector.Direction = newDirection;
        }
    }
}