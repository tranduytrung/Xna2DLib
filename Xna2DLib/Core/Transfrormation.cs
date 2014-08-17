using System;

namespace tranduytrung.Xna.Core
{
    public class Transfrormation
    {
        private float _rotate;
        private const float Tolerance = 1e-6f;
        private float _scaleX;
        private float _scaleY;
        private int _translateX;
        private int _translateY;
        private float _originX;
        private float _originY;

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

        public bool IsChanged { get; set; }

        public float ScaleX
        {
            get { return _scaleX; }
            set
            {
                if (Math.Abs(_scaleX - value) > Tolerance)
                {
                    _scaleX = value;
                    IsChanged = true;
                }
            }
        }


        public float ScaleY
        {
            get { return _scaleY; }
            set
            {
                if (Math.Abs(_scaleY - value) > Tolerance)
                {
                    _scaleY = value;
                    IsChanged = true;
                }
            }
        }

        public int TranslateX
        {
            get { return _translateX; }
            set
            {
                if (Math.Abs(_translateX - value) > Tolerance)
                {
                    _translateX = value;
                    IsChanged = true;
                }
            }
        }


        public int TranslateY
        {
            get { return _translateY; }
            set
            {
                if (Math.Abs(_translateY - value) > Tolerance)
                {
                    _translateY = value;
                    IsChanged = true;
                }
            }
        }

        public float OriginX
        {
            get { return _originX; }
            set
            {
                if (Math.Abs(_originX - value) > Tolerance)
                {
                    _originX = value;
                    IsChanged = true;
                }
            }
        }

        public float OriginY
        {
            get { return _originY; }
            set
            {
                if (Math.Abs(_originY - value) > Tolerance)
                {
                    _originY = value;
                    IsChanged = true;
                }
            }
        }

        public Transfrormation()
        {
            _scaleX = 1;
            _scaleY = 1;
            _rotate = 0.0f;
            _originX = 0.5f;
            _originY = 0.5f;
            _translateX = 0;
            _translateY = 0;
            IsChanged = true;
        }
    }
}
