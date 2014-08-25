using System;
using Microsoft.Xna.Framework;
using tranduytrung.Xna.Animation;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Engine;

namespace tranduytrung.DragonCity.Control
{
    public class Button : ContentPresenter
    {
        private Color _hoverColor = Color.Wheat;
        private readonly Storyboard _buttonDownStoryboard;
        private DrawableObject _background;

        public DrawableObject HoverBackground { get; set; }
        public DrawableObject PressBackground { get; set; }

        protected override void OnMouseEnter()
        {
            base.OnMouseEnter();

            _background = Background;
            if (HoverBackground != null)
            {
                Background = HoverBackground;
            }
            
            TintingColor = HoverColor;
        }

        protected override void OnMouseLeave()
        {
            base.OnMouseLeave();

            if (PressBackground != null && Input.IsMouseLeftPressed())
                Background = PressBackground;
            else
                Background = _background;
            
            TintingColor = Color.White;
        }

        public Color HoverColor
        {
            get { return _hoverColor; }
            set { _hoverColor = value; }
        }

        protected override void OnLeftMouseButtonDown(ref bool interupt)
        {
            if (PressBackground != null)
            {
                Background = PressBackground;
            }

            if (!AnimationManager.IsAnimating(_buttonDownStoryboard))
            {
                _buttonDownStoryboard.Reset();
                AnimationManager.BeginAnimation(_buttonDownStoryboard);
            }


            base.OnLeftMouseButtonDown(ref interupt);
        }

        protected override void OnRelease()
        {
            if (IsMouseOver && HoverBackground != null)
                Background = HoverBackground;
            else
                Background = _background;

            _buttonDownStoryboard.Reverse();
            if (!AnimationManager.IsAnimating(_buttonDownStoryboard))
            {
                AnimationManager.BeginAnimation(_buttonDownStoryboard);
            }

            base.OnRelease();
        }

        public override bool MouseInput(Vector2 relativePoint)
        {
            base.MouseInput(relativePoint);
            return true;
        }


        public Button()
        {
            Transform = new Transfrormation();
            _buttonDownStoryboard = new Storyboard();

            var scaleXAnimation = new FloatAnimation(this, "Transform.ScaleX")
            {
                From = 1f,
                To = 0.95f,
                Duration = TimeSpan.FromMilliseconds(100)
            };

            var scaleYAnimation = new FloatAnimation(this, "Transform.ScaleY")
            {
                From = 1f,
                To = 0.95f,
                Duration = TimeSpan.FromMilliseconds(100)
            };


            _buttonDownStoryboard.Animations.Add(scaleXAnimation);
            _buttonDownStoryboard.Animations.Add(scaleYAnimation);

            EnableMouseEvent = true;
        }

        public override void Dispose()
        {
            AnimationManager.EndAnimation(_buttonDownStoryboard);
            base.Dispose();
        }
    }

}
