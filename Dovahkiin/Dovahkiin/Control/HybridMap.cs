using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Map;

namespace Dovahkiin.Control
{
    public class HybridMap : IsometricMap
    {
        private readonly List<DrawableObject> _canvasObjectCollection = new List<DrawableObject>();

        public static readonly AttachableProperty XProperty = AttachableProperty.RegisterProperty(typeof (int), 0);
        public static readonly AttachableProperty YProperty = AttachableProperty.RegisterProperty(typeof(int), 0);

        public List<DrawableObject> CanvasObjectCollection
        {
            get { return _canvasObjectCollection; }
        }

        public HybridMap(int rowCount, int columnCount, int cellWidth, int cellHeight, Color[] colorMap)
            : base(rowCount, columnCount, cellWidth, cellHeight, colorMap)
        {
        }

        public override void Measure(Size availableSize)
        {
            base.Measure(availableSize);
            foreach (var canvasObject in CanvasObjectCollection)
            {
                canvasObject.Measure(new Size(availableSize.Width, availableSize.Height));
            }
        }

        public override void Arrange(Rectangle finalRectangle)
        {
            base.Arrange(finalRectangle);

            foreach (var canvasObject in CanvasObjectCollection)
            {
                var x = (int) canvasObject.GetValue(XProperty);
                var y = (int)canvasObject.GetValue(YProperty);

                canvasObject.Arrange(new Rectangle(x - canvasObject.DesiredWidth/2, y - canvasObject.DesiredHeight,
                    canvasObject.DesiredWidth, canvasObject.DesiredHeight));
            }
        }

        public override void PrepareVisual()
        {
            base.PrepareVisual();
            foreach (var canvasObject in CanvasObjectCollection)
            {
                canvasObject.PrepareVisual();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach (var canvasObject in CanvasObjectCollection)
            {
                canvasObject.Draw(spriteBatch);
            }
        }

        public override void RenderTransform()
        {
            base.RenderTransform();

            foreach (var canvasObject in CanvasObjectCollection)
            {
                canvasObject.RenderTransform();
            }
        }

        public override void Update()
        {
            foreach (var canvasObject in CanvasObjectCollection)
            {
                canvasObject.Update();
            }
        }

        public override bool MouseInputCore(Vector2 relativePoint)
        {
            var interupt = base.MouseInputCore(relativePoint);
            if (IsMouseOver) return interupt;

            foreach (var canvasObject in CanvasObjectCollection)
            {
                var interactiveObj = canvasObject as InteractiveObject;
                if (interactiveObj == null) continue;
                interactiveObj.ParentNotHit();
            }

            return interupt;
        }

        protected override bool HittedMouseProcess(Vector2 relativePoint)
        {
            var result = base.HittedMouseProcess(relativePoint);

            // Process children
            if (!EnableInteractiveChildren) return false;
            for (var i = _canvasObjectCollection.Count - 1; i >= 0; i--)
            {
                var interactiveObj = _canvasObjectCollection[i] as InteractiveObject;
                if (interactiveObj == null) continue;
                if (interactiveObj.MouseInputCore(new Vector2(relativePoint.X, relativePoint.Y)))
                {
                    return true;
                }
            }

            // =======================================================================

            return result;
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach (var canvasObject in CanvasObjectCollection)
            {
                canvasObject.Dispose();
            }
        }
    }
}