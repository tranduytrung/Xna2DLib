using System;
using Microsoft.Xna.Framework;
using tranduytrung.Xna.Control;
using tranduytrung.Xna.Core;

namespace GameMenu
{
    public class SpriteButton : Button
    {
        private Color _mouseOverColor = Color.Violet;
        private readonly Storyboard _buttonDownStoryboard;
        private readonly ContinuousAnimation _ratioAnimation;

        protected override void OnMouseEnter()
        {
            base.OnMouseEnter();

            var sprite = (Sprite)PresentableContent;
            sprite.TintingColor = MouseOverColor;
        }

        protected override void OnMouseLeave()
        {
            base.OnMouseLeave();

            var sprite = (Sprite)PresentableContent;
            sprite.TintingColor = Color.White;
        }

        public Color MouseOverColor
        {
            get { return _mouseOverColor; }
            set { _mouseOverColor = value; }
        }

        protected override void OnLeftMouseButtonDown(ref bool interupt)
        {
            if (!IsAnimating(_buttonDownStoryboard))
            {
                _ratioAnimation.From = 1;
                _ratioAnimation.To = 0.95;
                _buttonDownStoryboard.Reset();
                BeginAnimation(_buttonDownStoryboard);
            }
            

            base.OnLeftMouseButtonDown(ref interupt);
        }

        protected override void OnRelease()
        {
            _buttonDownStoryboard.Reverse();
            if (!IsAnimating(_buttonDownStoryboard))
            {
                BeginAnimation(_buttonDownStoryboard);
            }

            base.OnRelease();
        }

        public SpriteButton()
        {
            _buttonDownStoryboard = new Storyboard();

            _ratioAnimation = new ContinuousAnimation();
            _ratioAnimation.AnimationCallback += (value) =>
            {
                Transfrorm.Scale =
                    new Vector2((float)(double)value, (float)(double)value);
            };
            _ratioAnimation.Duration = TimeSpan.FromMilliseconds(100);

            _buttonDownStoryboard.Animations.Add(_ratioAnimation);
            Transfrorm = new Transfrormation();

            EnableMouseEvent = true;
        }
    }
}
