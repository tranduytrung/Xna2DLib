using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace tranduytrung.Xna.Core
{
    public abstract class DrawableObject : GameObject
    {
        /// <summary>
        /// Prepare the object visual. In this stage, object can change device's render targets
        /// </summary>
        public abstract void PrepareVisual();

        /// <summary>
        /// Draw the object visual using sprite batch without change to the other render targets
        /// </summary>
        /// <param name="spriteBatch">the sprite batch which is drawn to</param>
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract DrawableObject HitTestCore(Vector2 relativePoint);

        /// <summary>
        /// Measure desired width and desired height
        /// </summary>
        /// <param name="availableSize">the space for that object</param>
        public abstract void Measure(Size availableSize);


        /// <summary>
        /// Determines actual position
        /// </summary>
        /// <param name="finalRectangle">the rect in scrren where object is arranged to</param>
        public abstract void Arrange(Rectangle finalRectangle);

        /// <summary>
        /// Apply rendering state transformaton
        /// </summary>
        public abstract void RenderTransform();

        public Transfrormation Transfrorm { get; set; }

        public int ActualWidth { get; protected set; }

        public int ActualHeight { get; protected set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int DesiredWidth { get; protected set; }

        public int DesiredHeight { get; protected set; }

        public int RelativeX { get; protected set; }

        public int RelativeY { get; protected set; }

        protected DrawableObject()
        {
            Width = Height = int.MinValue;
        }
    }
}
