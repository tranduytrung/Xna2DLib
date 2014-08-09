using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tranduytrung.Xna.Engine;

namespace tranduytrung.Xna.Core
{
    public sealed class Sprite : DrawableObject
    {
        private readonly ISpriteSelector _selector;
        private Vector2 _scale = new Vector2(1,1);
        private float _rotate = 0.0f;
        private Vector2 _translate = Vector2.Zero;
        private Matrix _localTransformMatrix =  Matrix.Identity;
        private Matrix _actualTransformMatrix = Matrix.Identity;
        private Matrix? _invertedActualTransformMatrix;
        private Rectangle? _frameBounds;

        public Sprite(ISpriteSelector selector)
        {
            SpriteEffects = SpriteEffects.None;
            TintingColor = Color.White;
            _selector = selector;
            SelectorState = selector.GetFrane(new GameTime());
        }

        public Sprite(ISpriteSelector selector, int width, int height)
        {
            SpriteEffects = SpriteEffects.None;
            TintingColor = Color.White;
            Width = width;
            Height = height;
            _selector = selector;
            SelectorState = selector.GetFrane(new GameTime());
        }

        public SpriteMode SpriteMode { get; set; }

        public SpriteSelectorState SelectorState { get; private set; }

        public Color TintingColor { get; set; }

        public SpriteEffects SpriteEffects { get; set; }

        private Matrix InvertedActualTransformMatrix
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

        private Matrix ActualTransformMatrix
        {
            get { return _actualTransformMatrix; }
            set
            {
                _actualTransformMatrix = value;
                _invertedActualTransformMatrix = null;
            }
        }

        private Matrix LocalTransformMatrix
        {
            get { return _localTransformMatrix; }
            set { _localTransformMatrix = value; }
        }

        public override void Measure(Size availableSize)
        {
            switch (SpriteMode)
            {
                case SpriteMode.Original:
                    DesiredWidth = Width == int.MinValue ? SelectorState.Texture.Width : Width;
                    DesiredHeight = Height == int.MinValue ? SelectorState.Texture.Height : Height;
                    break;
                case SpriteMode.Fit:
                    DesiredWidth = Width == int.MinValue ? availableSize.Width : Width;
                    DesiredHeight = Height == int.MinValue ? availableSize.Height : Height;
                    break;
                case SpriteMode.FitHorizontal:
                    DesiredWidth = Width == int.MinValue ? availableSize.Width : Width;
                    DesiredHeight = Height == int.MinValue ? (int)(SelectorState.Texture.Height * ((float)DesiredWidth / SelectorState.Texture.Width)) : Height;
                    break;
                case SpriteMode.FitVertical:
                    DesiredHeight = Height == int.MinValue ? availableSize.Height : Height;
                    DesiredWidth = Height == int.MinValue ? (int)(SelectorState.Texture.Width * ((float)DesiredHeight / SelectorState.Texture.Height)) : Width;
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Arrange(Rectangle finalRectangle)
        {
            ActualWidth = finalRectangle.Width;
            ActualHeight = finalRectangle.Height;
            RelativeX = finalRectangle.X;
            RelativeY = finalRectangle.Y;

            // Mode
            float ratio, hPart, vPart;
            switch (SpriteMode)
            {
                case SpriteMode.Original:
                    _frameBounds = new Rectangle(SelectorState.ClipBounds.X, SelectorState.ClipBounds.Y,
                        finalRectangle.Width < SelectorState.ClipBounds.Width ? finalRectangle.Width : SelectorState.ClipBounds.Width,
                        finalRectangle.Height < SelectorState.ClipBounds.Height ? finalRectangle.Height : SelectorState.ClipBounds.Height);
                    break;
                case SpriteMode.Fit:
                    hPart = Math.Max((float)finalRectangle.Width / DesiredWidth, 1);
                    vPart = Math.Max((float)finalRectangle.Height / DesiredHeight, 1);

                    _frameBounds = new Rectangle(SelectorState.ClipBounds.X, SelectorState.ClipBounds.Y,
                        (int)(SelectorState.ClipBounds.Width * hPart),
                        (int)(SelectorState.ClipBounds.Height * vPart));
                    break;
                case SpriteMode.FitHorizontal:
                    ratio = Math.Max((float)DesiredWidth / SelectorState.ClipBounds.Width, 1);
                    hPart = Math.Max((float)finalRectangle.Width / DesiredWidth, 1);
                    vPart = Math.Max((float)finalRectangle.Height / DesiredHeight, 1);

                    _frameBounds = new Rectangle(SelectorState.ClipBounds.X, SelectorState.ClipBounds.Y,
                        (int)(SelectorState.ClipBounds.Width * hPart),
                        (int)(SelectorState.ClipBounds.Height * vPart * ratio));
                    break;
                case SpriteMode.FitVertical:
                    ratio = Math.Max((float)DesiredHeight / SelectorState.ClipBounds.Height, 1);
                    hPart = Math.Max((float)finalRectangle.Width / DesiredWidth, 1);
                    vPart = Math.Max((float)finalRectangle.Height / DesiredHeight, 1);

                    _frameBounds = new Rectangle(SelectorState.ClipBounds.X, SelectorState.ClipBounds.Y,
                        (int)(SelectorState.ClipBounds.Width * ratio * hPart),
                        (int)(SelectorState.ClipBounds.Height * vPart));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void RenderTransform()
        {
            if (Transfrorm != null)
            {
                if (Transfrorm.IsChanged)
                {
                    LocalTransformMatrix = Matrix.CreateTranslation(Transfrorm.TransformOrigin.X, Transfrorm.TransformOrigin.Y, 0) *
                       Matrix.CreateScale(Transfrorm.Scale.X, Transfrorm.Scale.Y, 1) *
                       Matrix.CreateRotationZ(Transfrorm.Rotate) *
                       Matrix.CreateTranslation(Transfrorm.Translate.X, Transfrorm.Translate.Y, 0);
                    ActualTransformMatrix = LocalTransformMatrix;
                    _invertedActualTransformMatrix = null;
                }

                _scale = Transfrorm.Scale;
                _rotate = Transfrorm.Rotate;
                _translate = Transfrorm.Translate;
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
            //        _scale = Transfrorm.Scale;
            //        _rotate = Transfrorm.Rotate;
            //        _translate = Transfrorm.Translate;
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

        public override DrawableObject HitTestCore(Vector2 relativePoint)
        {
            var localPoint = new Vector2(relativePoint.X - RelativeX, relativePoint.Y - RelativeY);
            var actualPoint = Vector2.Transform(localPoint, InvertedActualTransformMatrix);

            var isHit = actualPoint.X >= 0 && actualPoint.X < ActualWidth
                && actualPoint.Y >= 0 && actualPoint.Y < ActualHeight;

            return isHit ? this : null;
        }

        /// <summary>
        /// Update object state
        /// </summary>
        public override void Update()
        {
            var state = _selector.GetFrane(GlobalGameState.GameTime);
            SelectorState = state;
        }

        public override void PrepareVisual()
        {
        }

        /// <summary>
        /// Draw sprite to device
        /// </summary>
        /// <param name="spriteBatch">spite batch to draw to screen</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            var destination = new Rectangle((int)(RelativeX + _translate.X), (int)(RelativeY + _translate.Y),
                (int)(ActualWidth * _scale.X), (int)(ActualHeight * _scale.Y));

            // Draw to batch
            spriteBatch.Draw(SelectorState.Texture, destination, _frameBounds, TintingColor, _rotate, Vector2.Zero, SpriteEffects, 0);
        }
    }

    public enum SpriteMode
    {
        Original,
        Fit,
        FitHorizontal,
        FitVertical
    }
}
