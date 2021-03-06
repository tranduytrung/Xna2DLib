﻿using System;
using Dovahkiin.Constant;
using Microsoft.Xna.Framework;
using tranduytrung.Xna.Animation;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Engine;

namespace Dovahkiin.Control
{
    public class ToggleButton : ContentPresenter
    {
        public static readonly AttachableProperty TagProperty = AttachableProperty.RegisterProperty(typeof (object));

        private Color _hoverColor = Color.White;
        private readonly Storyboard _buttonDownStoryboard;
        private bool _isToggled;
        public DrawableObject HoverBackground { get; set; }
        public DrawableObject ToggledBackground { get; set; }
        public DrawableObject NormalBackground { get; set; }

        public object Tag
        {
            get { return GetValue(TagProperty); }
            set { SetValue(TagProperty, value); }
        }

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
            Background = IsToggled? ToggledBackground : NormalBackground;
            var handler = ToggleChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }


        protected override void OnMouseEnter()
        {
            Sounds.ButtonHover();
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

        protected override bool OnLeftMouseButtonDown(Vector2 relativePoint)
        {
            if (!AnimationManager.IsAnimating(_buttonDownStoryboard))
            {
                _buttonDownStoryboard.Reset();
                AnimationManager.BeginAnimation(_buttonDownStoryboard);
            }


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
            Sounds.ButtonClick();
            IsToggled = !IsToggled;
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
        }

        public override void Dispose()
        {
            AnimationManager.EndAnimation(_buttonDownStoryboard);
            base.Dispose();
        }

    }
}
