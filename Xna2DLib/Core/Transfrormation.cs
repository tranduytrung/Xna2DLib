using System;
using Microsoft.Xna.Framework;

namespace tranduytrung.Xna.Core
{
    public class Transfrormation
    {
        private Vector2 _scale;
        private float _rotate;
        private Vector2 _translate;
        private Vector2 _transformOrigin;
        private const float Tolerance = 1e-6f;

        public Vector2 Scale
        {
            get { return _scale; }
            set
            {
                if (value != _scale)
                {
                    _scale = value;
                    IsChanged = true;
                }
            }
        }

        /// <summary>
        /// Rotation angle in radian
        /// </summary>
        public float Rotate
        {
            get { return _rotate; }
            set
            {
                if (Math.Abs(value - _rotate) > Tolerance)
                {
                    _rotate = value;
                    IsChanged = true;
                }
            }
        }

        public Vector2 Translate
        {
            get { return _translate; }
            set
            {
                if (value != _translate)
                {
                    _translate = value;
                    IsChanged = true;
                }
            }
        }

        /// <summary>
        /// Calculate origin of transformation that left top is (0,0) and right bottom is (1,1)
        /// </summary>
        public Vector2 TransformOrigin
        {
            get { return _transformOrigin; }
            set
            {
                if (value != _transformOrigin)
                {
                    _transformOrigin = new Vector2(MathHelper.Clamp(value.X, 0, 1), MathHelper.Clamp(value.Y, 0, 1));
                    IsChanged = true;
                }
            }
        }

        public bool IsChanged { get; set; }

        public Transfrormation()
        {
            _scale = new Vector2(1,1);
            _rotate = 0.0f;
            _transformOrigin = Vector2.Zero;
            _translate = Vector2.Zero;
            IsChanged = true;
        }
    }
}
