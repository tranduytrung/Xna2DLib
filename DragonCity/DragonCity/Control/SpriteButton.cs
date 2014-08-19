using System;
using Microsoft.Xna.Framework;
using tranduytrung.Xna.Animation;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Engine;

namespace tranduytrung.DragonCity.Control
{
    public class SpriteButton : ContentPresenter
    {
        private Color _mouseOverColor = Color.Violet;
        private readonly Storyboard _buttonDownStoryboard;

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
            if (!AnimationManager.IsAnimating(_buttonDownStoryboard))
            {
                _buttonDownStoryboard.Reset();
                AnimationManager.BeginAnimation(_buttonDownStoryboard);
            }


            base.OnLeftMouseButtonDown(ref interupt);
        }

        protected override void OnRelease()
        {
            _buttonDownStoryboard.Reverse();
            if (!AnimationManager.IsAnimating(_buttonDownStoryboard))
            {
                AnimationManager.BeginAnimation(_buttonDownStoryboard);
            }

            base.OnRelease();
        }

        public SpriteButton()
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
