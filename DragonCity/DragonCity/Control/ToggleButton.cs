﻿using System;
using Microsoft.Xna.Framework;
using tranduytrung.Xna.Animation;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Engine;

namespace tranduytrung.DragonCity.Control
{
    public class ToggleButton : ContentPresenter
    {
        private Color _hoverColor = Color.Wheat;
        private readonly Storyboard _buttonDownStoryboard;
        private bool _isToggled;
        public DrawableObject HoverBackground { get; set; }
        public DrawableObject ToggledBackground { get; set; }
        public DrawableObject NormalBackground { get; set; }

        public bool IsToggled
        {
            get { return _isToggled; }
            set
            {
                if (value == _isToggled) return;
                _isToggled = value;
                OnToggleChanged();
            }
        }

        public event EventHandler<EventArgs> ToggleChanged;

        protected virtual void OnToggleChanged()
        {
            EventHandler<EventArgs> handler = ToggleChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }


        protected override void OnMouseEnter()
        {
            base.OnMouseEnter();

            Background = HoverBackground;
            TintingColor = HoverColor;
        }

        protected override void OnMouseLeave()
        {
            base.OnMouseLeave();

            Background = IsToggled ? ToggledBackground : NormalBackground;
            
            TintingColor = Color.White;
        }

        public Color HoverColor
        {
            get { return _hoverColor; }
            set { _hoverColor = value; }
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

        protected override void OnClick()
        {
            IsToggled = !IsToggled;
        }

        public override bool MouseInput(Vector2 relativePoint)
        {
            base.MouseInput(relativePoint);
            return true;
        }


        public ToggleButton()
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
