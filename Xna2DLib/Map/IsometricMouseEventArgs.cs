using System;
using System.Collections.Generic;
using tranduytrung.Xna.Core;

namespace tranduytrung.Xna.Map
{
    public class IsometricMouseEventArgs : EventArgs
    {
        /// <summary>
        /// Mouse's position in isometric coordinate
        /// </summary>
        public IsometricCoords Coordinate { get; private set; }

        /// <summary>
        /// Items in the cell
        /// </summary>
        public IEnumerable<DrawableObject> Items { get; private set; }

        /// <summary>
        /// Relative x position in cell, range from 0 to 1
        /// </summary>
        public double CellX { get; private set; }

        /// <summary>
        /// Relative y position in cell, range from 0 to 1
        /// </summary>
        public double CellY { get; private set; }

        /// <summary>
        /// Initialize an mouse event argument
        /// </summary>
        /// <param name="coordinate">Mouse's position in isometric coordinate</param>
        /// <param name="cellX">Items in the cell</param>
        /// <param name="cellY"> Relative x position in cell, range from 0 to 1</param>
        /// <param name="items"> Relative y position in cell, range from 0 to 1</param>
        public IsometricMouseEventArgs(IsometricCoords coordinate,  double cellX, double cellY, IEnumerable<DrawableObject> items)
        {
            CellY = cellY;
            CellX = cellX;
            Items = items;
            Coordinate = coordinate;
        }
    }
}
