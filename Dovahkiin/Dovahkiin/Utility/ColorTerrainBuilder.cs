using System.Linq;
using Dovahkiin.Maps;
using Dovahkiin.Model.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Map;
using Dovahkiin.Extension;

namespace Dovahkiin.Utility
{

    public static class ColorTerrainBuilder
    {
        /// <summary>
        /// Build the terrain base on color of texture
        /// </summary>
        /// <param name="mapModel">model of map</param>
        /// <param name="colorKeyTexture">color key texture</param>
        /// <param name="textureDictionary">mapping from tile's name to texture</param>
        /// <returns>collection of sprite in dictionary order</returns>
        public static IsometricMap CreateMap(Map mapModel, Texture2D colorKeyTexture, IDictionary<string, Texture2D> textureDictionary)
        {
            var rows = mapModel.RowCount;
            var cols = mapModel.ColumnCount;
            var cellWidth = colorKeyTexture.Width;
            var cellHeight = colorKeyTexture.Height;
            var colorKey = new Color[cellWidth*cellHeight];
            colorKeyTexture.GetData(colorKey);

            var map = new IsometricMap(rows, cols, cellWidth, cellHeight, colorKey);
            map.Width = cellWidth * (cols - 1) / 2;
            map.Height = cellHeight * (rows - 1) / 2;

            for (var row = 0; row < rows; ++row)
            {
                for (var col = 0; col < cols; ++col)
                {
                    if (row % 2 != col % 2)
                        continue;

                    var tileModel = mapModel.GetTileAt(row, col);

                    Texture2D texture;
                    if (!textureDictionary.TryGetValue(tileModel.Name, out texture))
                    {
                        throw new KeyNotFoundException(string.Format("Cannot maps {0} to any texture",
                            tileModel.Name));
                    }

                    var deploy = new UnitDeployment();
                    deploy.Deploy(new IsometricCoords(col, row));
                    var sprite = new Sprite(new SingleSpriteSelector(texture));
                    sprite.SetValue(IsometricMap.DeploymentProperty, deploy);
                    map.SetTile(sprite);
                }
            }

            return map;
        }
    }

}
