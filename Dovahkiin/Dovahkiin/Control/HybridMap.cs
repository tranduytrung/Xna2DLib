using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Map;

namespace Dovahkiin.Control
{
    public class HybridMap : IsometricMap
    {
        private bool _leftClickSeasion;
        private bool _rightClickSeasion;
        private Vector2 _leftMouseDownPosition;
        private Vector2 _rightMouseDownPosition;
        private readonly List<DrawableObject> _canvasObjectCollection = new List<DrawableObject>();

        public static readonly AttachableProperty XProperty = AttachableProperty.RegisterProperty(typeof (int), 0);
        public static readonly AttachableProperty YProperty = AttachableProperty.RegisterProperty(typeof(int), 0);
        

        public event EventHandler<MouseEventArgs> LeftMouseClick;
        public event EventHandler<MouseEventArgs> RightMouseClick;

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
            var baseX = -CellWidth / 2 + finalRectangle.X;
            var baseY = -CellHeight / 2 + finalRectangle.Y;

            foreach (var canvasObject in CanvasObjectCollection)
            {
                var x = baseX + (int)canvasObject.GetValue(XProperty);
                var y = baseY + (int)canvasObject.GetValue(YProperty);

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

        protected override bool OnLeftMouseButtonDown(Vector2 relativePoint)
        {
            var propagate = base.OnLeftMouseButtonDown(relativePoint);

            _leftClickSeasion = true;
            _leftMouseDownPosition = relativePoint;
            return propagate;
        }

        protected override bool OnLeftMouseButtonUp(Vector2 relativePoint)
        {
            var propagate = base.OnLeftMouseButtonDown(relativePoint);
            if (_leftClickSeasion && Vector2.DistanceSquared(_leftMouseDownPosition, relativePoint) < 10)
            {
                var localX = (int)(relativePoint.X - RelativeX) + CellWidth / 2;
                var localY = (int)(relativePoint.Y - RelativeY) + CellHeight / 2;
                OnLeftMouseClick(new MouseEventArgs(localX, localY));
            }

            return propagate;
        }

        protected override bool OnRightMouseButtonDown(Vector2 relativePoint)
        {
            var propagate = base.OnRightMouseButtonDown(relativePoint);

            _rightClickSeasion = true;
            _rightMouseDownPosition = relativePoint;

            return propagate;
        }

        protected override bool OnRightMouseButtonUp(Vector2 relativePoint)
        {
            var propagate = base.OnRightMouseButtonUp(relativePoint);
            if (_rightClickSeasion && Vector2.DistanceSquared(_rightMouseDownPosition, relativePoint) < 10)
            {
                var localX = (int)(relativePoint.X - RelativeX) + CellWidth / 2;
                var localY = (int)(relativePoint.Y - RelativeY) + CellHeight / 2;
                OnRightMouseClick(new MouseEventArgs(localX, localY));
            }

            return propagate;
        }

        protected virtual void OnLeftMouseClick(MouseEventArgs e)
        {
            EventHandler<MouseEventArgs> handler = LeftMouseClick;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnRightMouseClick(MouseEventArgs e)
        {
            EventHandler<MouseEventArgs> handler = RightMouseClick;
            if (handler != null) handler(this, e);
        }

        public override bool MouseInputCore(Vector2 relativePoint)
        {
            var propagate = base.MouseInputCore(relativePoint);
            if (IsMouseOver) return propagate;

            foreach (var canvasObject in CanvasObjectCollection)
            {
                var interactiveObj = canvasObject as InteractiveObject;
                if (interactiveObj == null) continue;
                interactiveObj.ParentNotHit();
            }

            return propagate;
        }

        protected override bool HittedMouseProcess(Vector2 relativePoint)
        {
            var propagate = base.HittedMouseProcess(relativePoint);

            // Process children
            if (!EnableInteractiveChildren) return true;
            for (var i = _canvasObjectCollection.Count - 1; i >= 0; i--)
            {
                var interactiveObj = _canvasObjectCollection[i] as InteractiveObject;
                if (interactiveObj == null) continue;
                if (interactiveObj.MouseInputCore(new Vector2(relativePoint.X, relativePoint.Y)))
                {
                    return false;
                }
            }

            // =======================================================================

            return propagate;
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