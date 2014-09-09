using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tranduytrung.Xna.Core;

namespace tranduytrung.Xna.Map
{
    public class IsometricMap : InteractiveObject
    {
        private IsometricCoords _currentMouseCoordinate = new IsometricCoords(-1, -1);
        private double _relativeCellX;
        private double _relativeCellY;
        private bool _clickSeasion;
        private IsometricCoords _downMouseCoords;
        private int _autoIndex;

        private readonly DrawableObject[,] _baseMatrix;
        private readonly List<DrawableObject> _children = new List<DrawableObject>();

        /// <summary>
        /// Red #FF0000FF
        /// </summary>
        public static readonly Color LeftTopColorKey = new Color(255, 0, 0, 255);

        /// <summary>
        /// Green #00FF00FF
        /// </summary>
        public static readonly Color RightTopColorKey = new Color(0, 255, 0, 255);

        /// <summary>
        /// Blue #0000FFFF
        /// </summary>
        public static readonly Color LeftBottomColorKey = new Color(0, 0, 255, 255);

        /// <summary>
        /// Magenta #FF00FFFF
        /// </summary>
        public static readonly Color RightBottomColorKey = new Color(255, 255, 0, 255);

        /// <summary>
        /// White #FFFFFFFF
        /// </summary>
        public static readonly Color CenterColorKey = new Color(255, 255, 255, 255);

        /// <summary>
        /// Deployment strategy
        /// </summary>
        public static AttachableProperty DeploymentProperty =
            AttachableProperty.RegisterProperty(typeof (IIsometricDeployable));

        protected static AttachableProperty IndexProperty = AttachableProperty.RegisterProperty(typeof (int));

        public IEnumerable<DrawableObject> Children
        {
            get { return _children; }
        }

        public bool EnableInteractiveChildren { get; set; }
        public int CellWidth { get; private set; }
        public int CellHeight { get; private set; }
        public int RowCount { get; private set; }
        public int ColumnCount { get; private set; }
        public Color[] ColorMap{ get; private set; }

        public IsometricCoords CurrentMouseCoordinate
        {
            get { return _currentMouseCoordinate; }
        }

        public event EventHandler<IsometricMouseEventArgs> IsometricMouseMoved;
        public event EventHandler<IsometricMouseEventArgs> IsometricMouseCoordinateChanged;
        public event EventHandler<IsometricMouseEventArgs> IsometricMouseClick;

        public IsometricMap(int rowCount, int columnCount, int cellWidth, int cellHeight, Color[] colorMap)
        {
            _baseMatrix = new DrawableObject[columnCount / 2 + columnCount % 2, rowCount];

            ColorMap = colorMap;
            ColumnCount = columnCount;
            RowCount = rowCount;
            CellWidth = cellWidth;
            CellHeight = cellHeight;
            EnableMouseEvent = true;
            EnableInteractiveChildren = true;
        }

        public override void Measure(Size availableSize)
        {
            // Determine Map width and height
            DesiredWidth = Width != int.MinValue ? Width : (ColumnCount - 1)*CellWidth;
            DesiredHeight = Height != int.MinValue ? Height : (RowCount - 1)*CellHeight;

            // Measure children
            foreach (var child in Children)
            {
                var deploy = (IIsometricDeployable) child.GetValue(DeploymentProperty);
                var iWidth = deploy.Right.X - deploy.Left.X;
                //var iheight = deploy.Bottom.Y - deploy.Top.Y;
                int width;
                if (iWidth == 0)
                    width = CellWidth;
                else
                    width = CellWidth + iWidth*(CellWidth/2);

                //if (iheight <= 0)
                //    height = 0;
                //else
                //    height = CellHeight + iheight * (CellHeight / 2);

                child.Measure(new Size(width, int.MaxValue));
            }
        }

        public override void Arrange(Rectangle finalRectangle)
        {
            base.Arrange(finalRectangle);

            var baseX = -CellWidth/2 + finalRectangle.X;
            var baseY = -CellHeight/2 + finalRectangle.Y;

            foreach (var child in Children)
            {
                var deploy = (IIsometricDeployable)child.GetValue(DeploymentProperty);
                var x = baseX + deploy.Left.X*(CellWidth/2);
                var y = baseY + deploy.Bottom.Y*(CellHeight/2) + CellHeight - child.DesiredHeight;
                child.Arrange(new Rectangle(x, y, child.DesiredWidth, child.DesiredHeight));
            }

            foreach (var child in _baseMatrix)
            {
                var deploy = (IIsometricDeployable)child.GetValue(DeploymentProperty);
                var x = baseX + deploy.Left.X * (CellWidth / 2);
                var y = baseY + deploy.Bottom.Y * (CellHeight / 2) + CellHeight - child.DesiredHeight;
                child.Arrange(new Rectangle(x, y, child.DesiredWidth, child.DesiredHeight));
            }
        }

        public override void PrepareVisual()
        {
            // Prepare children visual first
            foreach (var child in _baseMatrix)
            {
                child.PrepareVisual();
            }

            foreach (var child in Children)
            {
                child.PrepareVisual();
            }

            //var graphicsDevice = GameContext.GraphicsDevice;

            //// create internal sprite batch if it is not existed
            //if (_spriteBatch == null)
            //{
            //    _spriteBatch = new SpriteBatch(graphicsDevice);
            //}

            //// if there are no render target, create a new one
            //if (_renderTarget == null)
            //{
            //    _renderTarget = new RenderTarget2D(graphicsDevice, ActualWidth, ActualHeight);
            //}
            //else
            //{
            //    // if render target does not fit, clear the old and create another fit with it
            //    if (_renderTarget.Width != ActualWidth || _renderTarget.Height != ActualHeight)
            //    {
            //        _renderTarget.Dispose();
            //        _renderTarget = new RenderTarget2D(graphicsDevice, ActualWidth, ActualHeight);
            //    }
            //}

            //// Save old targets
            //var oldRenderTargets = graphicsDevice.GetRenderTargets();
            //// Set our render target
            //graphicsDevice.SetRenderTarget(_renderTarget);

            //// Fill with background Color
            //graphicsDevice.Clear(Color.Black);

            //// Draw all children visual to this panel visual
            //_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            // Sort the children
            //_children.Sort((lhs, rhs) =>
            //{
            //    var deplyL = (IIsometricDeployable)lhs.GetValue(DeploymentProperty);
            //    var deplyR = (IIsometricDeployable)rhs.GetValue(DeploymentProperty);
            //    var result = (deplyL.Bottom.Y * CellWidth + deplyL.Bottom.X) -
            //                 (deplyR.Bottom.Y * CellWidth + deplyR.Bottom.X);

            //    if (result == 0)
            //        return (int)lhs.GetValue(IndexProperty) - (int)rhs.GetValue(IndexProperty);

            //    return result;
            //});

            //foreach (var child in Children)
            //{
            //    child.Draw(_spriteBatch);
            //}

            //_spriteBatch.End();

            //// Restore targets
            //graphicsDevice.SetRenderTargets(oldRenderTargets);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw to outer batch
            //var destination = new Rectangle((int)(RelativeX + ActualTranslate.X), (int)(RelativeY + ActualTranslate.Y),
            //    (int)(ActualWidth * ActualScale.X), (int)(ActualHeight * ActualScale.Y));

            for (int x = 0; x < _baseMatrix.GetLength(0); x++)
            {
                for (int y = 0; y < _baseMatrix.GetLength(1); y++)
                {
                    if (_baseMatrix[x, y] == null) continue;
                    _baseMatrix[x, y].Draw(spriteBatch);
                }
            }

            foreach (var child in Children)
            {
                child.Draw(spriteBatch);
            }
        }

        public override void RenderTransform()
        {
            base.RenderTransform();

            foreach (var child in Children)
            {
                child.RenderTransform();
            }
        }

        public override void Update()
        {
            foreach (var child in Children)
            {
                child.Update();
            }
        }
        public override bool MouseInputCore(Vector2 relativePoint)
        {
            var interupt = base.MouseInputCore(relativePoint);
            if (IsMouseOver) return interupt;

            foreach (var child in Children)
            {
                var interactiveObj = child as InteractiveObject;
                if (interactiveObj == null) continue;
                interactiveObj.ParentNotHit();
            }

            return interupt;
        }

        protected override bool HittedMouseProcess(Vector2 relativePoint)
        {
            if (base.HittedMouseProcess(relativePoint))
                return true;

            // Isometric Coordinate ==============================================
            ProcessMouse(relativePoint);

            // Process children
            if (!EnableInteractiveChildren) return false;
            for (var i = _children.Count - 1; i >= 0; i--)
            {
                var interactiveObj = _children[i] as InteractiveObject;
                if (interactiveObj == null) continue;
                if (interactiveObj.MouseInputCore(new Vector2(relativePoint.X, relativePoint.Y)))
                {
                    return true;
                }
            }

            // =======================================================================
            
            return false;
        }

        private void ProcessMouse(Vector2 relativePoint)
        {
            // local position in map
            var localX = (int)(relativePoint.X - RelativeX) + CellWidth / 2;
            var localY = (int)(relativePoint.Y - RelativeY) + CellHeight / 2;
            
            // This version cannot operate if local position is negative
            if (localX < 0 || localY <0)
                return;
            
            // Map's coordinate position
            var isometricX = (localX / CellWidth)*2;
            var isometricY = (localY / CellHeight)*2;

            // In-cell position
            var cellX = localX % CellWidth;
            var cellY = localY % CellHeight;

            // lookup index
            var index = cellY * CellWidth + cellX;

            // get color in color map
            var key = ColorMap[index];

            // modify position
            if (key == LeftTopColorKey)
            {
                --isometricX;
                --isometricY;
                _relativeCellX = (double)cellX / CellWidth + 0.5;
                _relativeCellY = (double)cellY / CellHeight + 0.5;
            }
            else if (key == RightTopColorKey)
            {
                ++isometricX;
                --isometricY;
                _relativeCellX = (double)cellX / CellWidth - 0.5;
                _relativeCellY = (double)cellY / CellHeight + 0.5;
            }
            else if (key == LeftBottomColorKey)
            {
                --isometricX;
                ++isometricY;
                _relativeCellX = (double)cellX / CellWidth + 0.5;
                _relativeCellY = (double)cellY / CellHeight - 0.5;
            }
            else if (key == RightBottomColorKey)
            {
                ++isometricX;
                ++isometricY;
                _relativeCellX = (double)cellX / CellWidth - 0.5;
                _relativeCellY = (double)cellY / CellHeight - 0.5;
            }
            else if (key == CenterColorKey)
            {
                _relativeCellX = (double) cellX/CellWidth;
                _relativeCellY = (double) cellY/CellHeight;
            }
            else
            {
                throw new InvalidOperationException("Color map is not valid.\nLeft Top: Red\nRight Top: Green\nLeft Bottom: Blue\nRight Bottom: Magenta\nCenter: White");
            }

            // check if position is changed
            if (CurrentMouseCoordinate.X != isometricX || CurrentMouseCoordinate.Y != isometricY)
            {
                _currentMouseCoordinate = new IsometricCoords(isometricX, isometricY);
                //Console.WriteLine("({0}, {1})", isometricX, isometricY);
                if (IsometricMouseCoordinateChanged != null)
                {
                    var inPlaceObj = GetChildren(CurrentMouseCoordinate.X, CurrentMouseCoordinate.Y);
                    IsometricMouseCoordinateChanged.Invoke(this, new IsometricMouseEventArgs(CurrentMouseCoordinate, _relativeCellX, _relativeCellY, inPlaceObj));
                }
            }

            // Mouse moved
            if (IsometricMouseMoved != null && Input.IsMouseMoved())
            {
                var inPlaceObj = GetChildren(CurrentMouseCoordinate.X, CurrentMouseCoordinate.Y);
                IsometricMouseMoved.Invoke(this, new IsometricMouseEventArgs(CurrentMouseCoordinate, _relativeCellX, _relativeCellY, inPlaceObj));
            }
        }

        protected override bool OnLeftMouseButtonDown(Vector2 relativePoint)
        {
            _clickSeasion = true;
            _downMouseCoords = CurrentMouseCoordinate;

            return true;
        }

        protected override bool OnLeftMouseButtonUp(Vector2 relativePoint)
        {
            if (_clickSeasion)
            {
                if (_downMouseCoords == CurrentMouseCoordinate)
                {
                    if (IsometricMouseClick != null)
                    {
                        var inPlaceObj = GetChildren(CurrentMouseCoordinate.X, CurrentMouseCoordinate.Y);
                        IsometricMouseClick.Invoke(this, new IsometricMouseEventArgs(CurrentMouseCoordinate, _relativeCellX, _relativeCellY, inPlaceObj));
                    }
                }

                _clickSeasion = false;
            }

            return true;
        }

        public IEnumerable<DrawableObject> GetChildren(int isometricX, int isometricY)
        {
            var coordinate = new IsometricCoords(isometricX, isometricY);
            int x, y;
            IsometricToMatrix(coordinate, out x, out y);
            var tile = _baseMatrix[x, y];
            var result = from child in Children
                        where
                            ((IIsometricDeployable)child.GetValue(DeploymentProperty)).Formation.Any(
                                coord => coord == coordinate)
                        select child;

            return tile != null ? result.Concat(new[] { tile }) : result;
        }

        public void AddChild(DrawableObject obj)
        {
            obj.SetValue(IndexProperty, _autoIndex++);
            _children.Add(obj);
        }

        public void RemoveChild(DrawableObject obj)
        {
            _children.Remove(obj);
        }

        private static void IsometricToMatrix(IsometricCoords coords, out int x, out int y)
        {
            x = coords.X / 2;
            y = coords.Y;
        }

        public void SetTile(DrawableObject obj)
        {
            var deploy = (UnitDeployment)obj.GetValue(IsometricMap.DeploymentProperty);
            int x, y;
            IsometricToMatrix(deploy.Left, out x, out y);
            _baseMatrix[y, x] = obj;
            obj.Measure(new Size(CellWidth, int.MaxValue));
        }

        public override void Dispose()
        {
            foreach (var item in _baseMatrix)
            {
                item.Dispose();
            }

            foreach (var child in Children)
            {
                child.Dispose();
            }

            base.Dispose();
        }
    }
}
