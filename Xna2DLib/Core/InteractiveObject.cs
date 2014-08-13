﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using tranduytrung.Xna.Engine;

namespace tranduytrung.Xna.Core
{
    public abstract class InteractiveObject : DrawableObject
    {
        private bool _isMouseOver;
        private readonly HashSet<Storyboard> _storyboardCollection = new HashSet<Storyboard>();
        private Matrix? _invertedActualTransformMatrix;

        protected InteractiveObject()
        {
            ActualScale = new Vector2(1, 1);
            ActualTranslate = Vector2.Zero;
            ActualRotate = 0.0f;
        }

        public bool IsMouseOver
        {
            get { return _isMouseOver; }
            private set
            {
                if (_isMouseOver != value)
                {
                    _isMouseOver = value;
                    if (IsMouseOverChanged != null)
                    {
                        IsMouseOverChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        protected Matrix InvertedActualTransformMatrix
        {
            get
            {
                if (_invertedActualTransformMatrix == null)
                {
                    //_invertedActualTransformMatrix = Matrix.Invert(ActualTransformMatrix);
                    return Matrix.Identity;
                }
                return _invertedActualTransformMatrix.Value;
            }
        }

        public bool EnableMouseEvent { get; set; }

        protected float ActualRotate { get; private set; }

        protected Vector2 ActualTranslate { get; private set; }

        protected Vector2 ActualScale { get; private set; }

        protected Matrix ActualTransformMatrix { get; private set; }

        public void BeginAnimation(Storyboard storyboard)
        {
            _storyboardCollection.Add(storyboard);
        }

        public void EndAnimation(Storyboard storyboard)
        {
            _storyboardCollection.Remove(storyboard);
        }

        public bool IsAnimating(Storyboard storyboard)
        {
            return _storyboardCollection.Contains(storyboard);
        }

        public event EventHandler<MouseEventArgs> MouseEnter;
        public event EventHandler<MouseEventArgs> MouseLeave;
        public event EventHandler<MouseEventArgs> LeftMouseButtonDown;
        public event EventHandler<MouseEventArgs> LeftMouseButtonUp;
        public event EventHandler<MouseEventArgs> LeftMousePressed;
        public event EventHandler<EventArgs> IsMouseOverChanged;

        protected virtual void OnLeftMouseButtonDown(ref bool interupt)
        {
        }

        protected virtual void OnLeftMouseButtonUp(ref bool interupt)
        {
        }

        protected virtual void OnMouseLeave()
        {
        }

        protected virtual void OnMouseEnter()
        {
        }

        public virtual void OnLeftMouseButtonPressed(ref bool interupt)
        {
        }

        /// <summary>
        /// Process mouse input
        /// </summary>
        /// <returns>Return true if the control want the process is stop bubbling up the tree</returns>
        public virtual bool MouseInput(Vector2 relativePoint)
        {
            if (!EnableMouseEvent)
                return false;

            bool isHit = HitTestCore(relativePoint) != null;

            if (!IsMouseOver && isHit)
            {
                IsMouseOver = true;
                OnMouseEnter();
                if (MouseEnter != null)
                {
                    MouseEnter(this, new MouseEventArgs());
                }
            }
            else if (IsMouseOver && !isHit)
            {
                IsMouseOver = false;
                OnMouseLeave();
                if (MouseLeave != null)
                {
                    MouseLeave(this, new MouseEventArgs());
                }
            }

            if (isHit)
            {
                var interupt = true;
                if (Input.IsLeftMouseButtonDown())
                {
                    OnLeftMouseButtonDown(ref interupt);
                    if (LeftMouseButtonDown != null)
                    {
                        LeftMouseButtonDown(this, new MouseEventArgs());
                    }
                }
                else if (Input.IsLeftMouseButtonUp())
                {
                    OnLeftMouseButtonUp(ref interupt);
                    if (LeftMouseButtonUp != null)
                    {
                        LeftMouseButtonUp(this, new MouseEventArgs());
                    }
                }
                else if (Input.IsMouseLeftPressed())
                {
                    OnLeftMouseButtonPressed(ref interupt);
                    if (LeftMousePressed != null)
                    {
                        LeftMousePressed(this, new MouseEventArgs());
                    }
                }
                else
                {
                    return false;
                }

                return interupt;
            }

            return false;
        }

        public override void Arrange(Rectangle finalRectangle)
        {
            ActualWidth = finalRectangle.Width;
            ActualHeight = finalRectangle.Height;
            RelativeX = finalRectangle.X;
            RelativeY = finalRectangle.Y;
        }

        public override void Measure(Size availableSize)
        {
            DesiredWidth = Width != int.MinValue ? Width : availableSize.Width;
            DesiredHeight = Height != int.MinValue ? Height : availableSize.Height;
        }

        public override DrawableObject HitTestCore(Vector2 relativePoint)
        {
            var localPoint = new Vector2(relativePoint.X - RelativeX, relativePoint.Y - RelativeY);
            var actualPoint = Vector2.Transform(localPoint, InvertedActualTransformMatrix);
            
            return HitTest(actualPoint);
        }

        public virtual DrawableObject HitTest(Vector2 localPoint)
        {
            var isHit = localPoint.X >= 0 && localPoint.X < ActualWidth
                && localPoint.Y >= 0 && localPoint.Y < ActualHeight;

            return isHit ? this : null;
        }

        public override void RenderTransform()
        {
            if (Transfrorm != null)
            {
                if (Transfrorm.IsChanged)
                {
                    ActualTransformMatrix =
                        Matrix.CreateTranslation(-Transfrorm.TransformOrigin.X*ActualWidth,
                            -Transfrorm.TransformOrigin.Y*ActualHeight, 0)*
                        Matrix.CreateScale(Transfrorm.Scale.X, Transfrorm.Scale.Y, 1)*
                        Matrix.CreateRotationZ(Transfrorm.Rotate)*
                        Matrix.CreateTranslation(Transfrorm.Translate.X + Transfrorm.TransformOrigin.X*ActualWidth,
                            Transfrorm.Translate.Y + Transfrorm.TransformOrigin.Y*ActualHeight, 0);

                    _invertedActualTransformMatrix = Matrix.Invert(ActualTransformMatrix);

                    Vector3 scale, translate;
                    Quaternion rotate;
                    ActualTransformMatrix.Decompose(out scale, out rotate, out translate);
                    Vector3 rPoint;
                    Vector3 point = Vector3.UnitX;
                    Vector3.Transform(ref point, ref rotate, out  rPoint);
                    ActualScale = new Vector2(scale.X, scale.Y);
                    ActualRotate = (float)Math.Atan2(rPoint.Y, rPoint.X);
                    ActualTranslate = new Vector2(translate.X, translate.Y);

                    Transfrorm.IsChanged = false;
                }
            }
            else
            {
                ActualScale = new Vector2(1, 1);
                ActualRotate = 0.0f;
                ActualTranslate = Vector2.Zero;
                _invertedActualTransformMatrix = Matrix.Identity;
            }

            //_invertedActualTransformMatrix = null;
            //if (Transfrorm != null)
            //{
            //    if (Transfrorm.IsChanged)
            //    {
            //        LocalTransformMatrix = Matrix.CreateTranslation(Transfrorm.TransformOrigin.X, Transfrorm.TransformOrigin.Y, 0) *
            //           Matrix.CreateScale(Transfrorm.Scale.X, Transfrorm.Scale.Y, 1) *
            //           Matrix.CreateRotationZ(Transfrorm.Rotate) *
            //           Matrix.CreateTranslation(Transfrorm.Translate.X, Transfrorm.Translate.Y, 0);
            //    }

            //    if (globalTransform == Matrix.Identity)
            //    {
            //        ActualScale = Transfrorm.Scale;
            //        ActualRotate = Transfrorm.Rotate;
            //        ActualTranslate = Transfrorm.Translate;
            //        ActualTransformMatrix = LocalTransformMatrix;
            //    }
            //    else
            //    {
            //        ActualTransformMatrix = globalTransform * LocalTransformMatrix;
            //        Vector3 scale, translate;
            //        Quaternion rotate;
            //        ActualTransformMatrix.Decompose(out scale, out rotate, out translate);
            //        Vector3 rPoint;
            //        Vector3 point = Vector3.UnitX;
            //        Vector3.Transform(ref point, ref rotate, out  rPoint);
            //        ActualScale = new Vector2(scale.X, scale.Y);
            //        ActualRotate = (float)Math.Atan2(rPoint.Y, rPoint.X);
            //        ActualTranslate = new Vector2(translate.X, translate.Y);
            //    }
            //}
            //else
            //{
            //    LocalTransformMatrix = Matrix.Identity;
            //    ActualTransformMatrix = globalTransform;
            //    if (globalTransform == Matrix.Identity)
            //    {
            //        ActualScale = new Vector2(1, 1);
            //        ActualRotate = 0.0f;
            //        ActualTranslate = Vector2.Zero;
            //    }
            //    else
            //    {
            //        Vector3 scale, translate;
            //        Quaternion rotate;
            //        globalTransform.Decompose(out scale, out rotate, out translate);
            //        Vector3 rPoint;
            //        Vector3 point = Vector3.UnitX;
            //        Vector3.Transform(ref point, ref rotate, out  rPoint);
            //        ActualScale = new Vector2(scale.X, scale.Y);
            //        ActualRotate = (float)Math.Atan2(rPoint.Y, rPoint.X);
            //        ActualTranslate = new Vector2(translate.X, translate.Y);
            //    }
            //}
        }

        public override void Update()
        {
            // Animation
            _storyboardCollection.RemoveWhere(obj => !obj.Update(GlobalGameState.GameTime.ElapsedGameTime));
        }
    }
}
