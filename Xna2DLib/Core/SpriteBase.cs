using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace tranduytrung.Xna.Core
{
    public abstract class SpriteBase : DrawableObject
    {
        protected SpriteBase()
        {
            ActualTransformMatrix = Matrix.Identity;
            ActualTranslate = Vector2.Zero;
            ActualRotate = 0.0f;
            ActualScale = new Vector2(1, 1);
            SpriteEffects = SpriteEffects.None;
            TintingColor = Color.White;
        }

        protected Vector2 ActualScale { get; set; }

        protected float ActualRotate { get; set; }

        protected Vector2 ActualTranslate { get; set; }

        public Matrix ActualTransformMatrix { get; set; }

        private Matrix? _invertedActualTransformMatrix;
        protected Matrix InvertedActualTransformMatrix
        {
            get
            {
                if (_invertedActualTransformMatrix == null)
                {
                    _invertedActualTransformMatrix = Matrix.Invert(ActualTransformMatrix);
                }
                return _invertedActualTransformMatrix.Value;
            }
        }


        public Color TintingColor { get; set; }

        public SpriteEffects SpriteEffects { get; set; }

        public override DrawableObject HitTestCore(Vector2 relativePoint)
        {
            var localPoint = new Vector2(relativePoint.X - RelativeX, relativePoint.Y - RelativeY);
            var actualPoint = Vector2.Transform(localPoint, InvertedActualTransformMatrix);

            var isHit = actualPoint.X >= 0 && actualPoint.X < ActualWidth
                && actualPoint.Y >= 0 && actualPoint.Y < ActualHeight;

            return isHit ? this : null;
        }

        public override void RenderTransform()
        {
            if (Transform != null)
            {
                if (Transform.IsChanged)
                {
                    ActualTransformMatrix =
                        Matrix.CreateTranslation(-Transform.OriginX * ActualWidth,
                            -Transform.OriginY * ActualHeight, 0) *
                        Matrix.CreateScale(Transform.ScaleX, Transform.ScaleY, 1) *
                        Matrix.CreateRotationZ(Transform.Rotate) *
                        Matrix.CreateTranslation(Transform.TranslateX + Transform.OriginX * ActualWidth,
                            Transform.TranslateY + Transform.OriginY * ActualHeight, 0);

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

                    Transform.IsChanged = false;
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
            //if (Transform != null)
            //{
            //    if (Transform.IsChanged)
            //    {
            //        LocalTransformMatrix = Matrix.CreateTranslation(Transform.TransformOrigin.X, Transform.TransformOrigin.Y, 0) *
            //           Matrix.CreateScale(Transform.Scale.X, Transform.Scale.Y, 1) *
            //           Matrix.CreateRotationZ(Transform.Rotate) *
            //           Matrix.CreateTranslation(Transform.Translate.X, Transform.Translate.Y, 0);
            //    }

            //    if (globalTransform == Matrix.Identity)
            //    {
            //        _scale = Transform.Scale;
            //        _rotate = Transform.Rotate;
            //        _translate = Transform.Translate;
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
            //        _scale = new Vector2(scale.X, scale.Y);
            //        _rotate = (float)Math.Atan2(rPoint.Y, rPoint.X);
            //        _translate = new Vector2(translate.X, translate.Y);
            //    }
            //}
            //else
            //{
            //    LocalTransformMatrix = Matrix.Identity;
            //    ActualTransformMatrix = globalTransform;
            //    if (globalTransform == Matrix.Identity)
            //    {
            //        _scale = new Vector2(1, 1);
            //        _rotate = 0.0f;
            //        _translate = Vector2.Zero;
            //    }
            //    else
            //    {
            //        Vector3 scale, translate;
            //        Quaternion rotate;
            //        globalTransform.Decompose(out scale, out rotate, out translate);
            //        Vector3 rPoint;
            //        Vector3 point = Vector3.UnitX;
            //        Vector3.Transform(ref point, ref rotate, out  rPoint);
            //        _scale = new Vector2(scale.X, scale.Y);
            //        _rotate = (float)Math.Atan2(rPoint.Y, rPoint.X);
            //        _translate = new Vector2(translate.X, translate.Y);
            //    }
            //}
        }

        public override void PrepareVisual()
        {
        }
    }
}
