using Dovahkiin.Control;
using Dovahkiin.Maps;
using Dovahkiin.Repository;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Map;

namespace Dovahkiin.Utility
{

    public static class ColorTerrainBuilder
    {
        /// <summary>
        /// Build the terrain base on color of texture
        /// </summary>
        /// <param name="mapModel">model of map</param>
        /// <param name="colorKeyTexture">color key texture</param>
        /// <returns>collection of sprite in dictionary order</returns>
        public static HybridMap CreateMap(Map mapModel, Texture2D colorKeyTexture)
        {
            var rows = mapModel.RowCount;
            var cols = mapModel.ColumnCount;
            var cellWidth = colorKeyTexture.Width;
            var cellHeight = colorKeyTexture.Height;
            var colorKey = new Color[cellWidth*cellHeight];
            colorKeyTexture.GetData(colorKey);

            var map = new HybridMap(rows, cols, cellWidth, cellHeight, colorKey)
            {
                Width = cellWidth*(cols - 1)/2,
                Height = cellHeight*(rows - 1)/2
            };

            for (var row = 0; row < rows; ++row)
            {
                for (var col = 0; col < cols; ++col)
                {
                    if (row % 2 != col % 2)
                        continue;

                    var tileModel = mapModel.GetTileAt(row, col);
                    var texture = Resouces.GetTexture(tileModel.ResouceId);

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
