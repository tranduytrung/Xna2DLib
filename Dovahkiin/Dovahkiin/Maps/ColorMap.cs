using System.Collections.Generic;
using Dovahkiin.Model.TileModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dovahkiin.Maps
{
    public class ColorMap : Map
    {
        private IDictionary<Color, Tile> _mappingDictionary;
        private Color[] _colorArray;

        private ColorMap(int rowCount, int columnCount, string name)
            : base(rowCount, columnCount, name)
        { 
        }

        protected override Tile ExtractTileAt(int row, int column)
        {
            var index = ColumnCount * row + column;
            var color = _colorArray[index];
            Tile tile;
            if (!_mappingDictionary.TryGetValue(color, out tile))
            {
                throw new KeyNotFoundException(string.Format("Cannot maps {0} to any texture",
                            _colorArray[index]));
            }

            return tile;
        }

        public static ColorMap CreateMap(string name, Texture2D colorMap, IDictionary<Color, Tile> mappingDictionary)
        {
            var map = new ColorMap(colorMap.Height, colorMap.Width, name);

            map._mappingDictionary = mappingDictionary;
            map._colorArray = new Color[colorMap.Width * colorMap.Height];
            colorMap.GetData(map._colorArray);

            map.Load();

            map._colorArray = null;
            map._mappingDictionary = null;

            return map;
        }
    }
}