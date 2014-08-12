using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Engine;

namespace tranduytrung.Xna.Map
{
    public class IsometricMap : InteractiveObject
    {
        private IsometricCoords _currentMouseCoords = new IsometricCoords(-1, -1);
        private double _relativeCellX;
        private double _relativeCellY;
        private bool _clickSeasion;
        private IsometricCoords _downMouseCoords;
        private int _autoIndex;

        private SpriteBatch _spriteBatch;
        private RenderTarget2D _renderTarget;
        private List<DrawableObject> _children = new List<DrawableObject>();

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
        public static readonly Color RightBottomColorKey = new Color(255, 0, 255, 255);

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

        public int CellWidth { get; private set; }
        public int CellHeight { get; private set; }
        public int RowCount { get; private set; }
        public int ColumnCount { get; private set; }
        public Color[] ColorMap{ get; private set; }

        public event EventHandler<IsometricMouseEventArgs> IsometricMouseChanged;
        public event EventHandler<IsometricMouseEventArgs> IsometricMouseClick;

        public IsometricMap(int rowCount, int columnCount, int cellWidth, int cellHeight, Color[] colorMap)
        {
            ColorMap = colorMap;
            ColumnCount = columnCount;
            RowCount = rowCount;
            CellWidth = cellWidth;
            CellHeight = cellHeight;
            EnableMouseEvent = true;
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
                int width, height;
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
        }

        public override void PrepareVisual()
        {
            // Prepare children visual first
            foreach (var child in Children)
            {
                child.PrepareVisual();
            }

            //var graphicsDevice = GlobalGameState.GraphicsDevice;

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
            _children.Sort((lhs, rhs) =>
            {
                var deplyL = (IIsometricDeployable)lhs.GetValue(DeploymentProperty);
                var deplyR = (IIsometricDeployable)rhs.GetValue(DeploymentProperty);
                var result = (deplyL.Bottom.Y * CellWidth + deplyL.Bottom.X) -
                             (deplyR.Bottom.Y * CellWidth + deplyR.Bottom.X);

                if (result == 0)
                    return (int)lhs.GetValue(IndexProperty) - (int)rhs.GetValue(IndexProperty);

                return result;
            });

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
            var destination = new Rectangle((int)(RelativeX + ActualTranslate.X), (int)(RelativeY + ActualTranslate.Y),
                (int)(ActualWidth * ActualScale.X), (int)(ActualHeight * ActualScale.Y));


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
            base.Update();

            foreach (var child in Children)
            {
                child.Update();
            }
        }

        public override bool MouseInput(Vector2 relativePoint)
        {
            // Process children
            for (var i = _children.Count - 1; i >= 0; i--)
            {
                var interactiveObj = _children[i] as InteractiveObject;
                if (interactiveObj == null) continue;
                if (interactiveObj.MouseInput(new Vector2(relativePoint.X - RelativeX, relativePoint.Y - RelativeY)))
                {
                    break;
                }
            }

            // Isometric Coordinate ==============================================
            ProcessMouse(relativePoint);

            // =======================================================================

            base.MouseInput(relativePoint);
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
            }
            else if (key == RightTopColorKey)
            {
                ++isometricX;
                --isometricY;
            }
            else if (key == LeftBottomColorKey)
            {
                --isometricX;
                ++isometricY;
            }
            else if (key == RightBottomColorKey)
            {
                ++isometricX;
                ++isometricY;
            }
            else if (key != CenterColorKey)
            {
                throw new InvalidOperationException("Color map is not valid.\nLeft Top: Red\nRight Top: Green\nLeft Bottom: Blue\nRight Bottom: Magenta\nCenter: White");
            }

            // Relative cell position
            _relativeCellX = (double) cellX/CellWidth;
            _relativeCellY = (double) cellY/CellHeight;

            // check if position is changed
            if (_currentMouseCoords.X != isometricX || _currentMouseCoords.Y != isometricY)
            {
                _currentMouseCoords = new IsometricCoords(isometricX, isometricY);
                //Console.WriteLine("({0}, {1})", isometricX, isometricY);
                if (IsometricMouseChanged != null)
                {
                    var inPlaceObj = from child in Children
                        where
                            ((IIsometricDeployable) child.GetValue(DeploymentProperty)).Formation.Any(
                                coord => coord == _currentMouseCoords)
                        select child;
                    IsometricMouseChanged.Invoke(this, new IsometricMouseEventArgs(_currentMouseCoords, _relativeCellX, _relativeCellY, inPlaceObj));
                }
            }
        }

        protected override void OnLeftMouseButtonDown(ref bool interupt)
        {
            _clickSeasion = true;
            _downMouseCoords = _currentMouseCoords;
        }

        protected override void OnLeftMouseButtonUp(ref bool interupt)
        {
            if (_clickSeasion)
            {
                if (_downMouseCoords == _currentMouseCoords)
                {
                    if (IsometricMouseClick != null)
                    {
                        var inPlaceObj = from child in Children
                                         where
                                             ((IIsometricDeployable)child.GetValue(DeploymentProperty)).Formation.Any(
                                                 coord => coord == _currentMouseCoords)
                                         select child;
                        IsometricMouseClick.Invoke(this, new IsometricMouseEventArgs(_currentMouseCoords, _relativeCellX, _relativeCellY, inPlaceObj));
                    }
                }

                _clickSeasion = false;
            }
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
    }
}
