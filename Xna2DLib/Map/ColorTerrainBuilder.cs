using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Engine;

namespace tranduytrung.Xna.Map
{
    public static class ColorTerrainBuilder
    {
        /// <summary>
        /// Build the terrain base on color of texture
        /// </summary>
        /// <param name="map">the map which will added to</param>
        /// <param name="source">the texture typed Texture2D</param>
        /// <param name="mappingDictionary">the mapping rules</param>
        /// <returns>collection of sprite in dictionary order</returns>
        public static IEnumerable<DrawableObject> BuildTerain(this IsometricMap map, Texture2D source, IDictionary<Color, Texture2D> mappingDictionary)
        {
            var sprites = new List<DrawableObject>();

            var colorSource = new Color[source.Width*source.Height];
            source.GetData(colorSource);

            for (var i = 0; i < source.Height; ++i)
            {
                for (var j = 0; j < source.Width; ++j)
                {
                    if (i%2 != j%2)
                        continue;

                    var index = source.Width*i + j;

                    Texture2D text;
                    if (!mappingDictionary.TryGetValue(colorSource[index], out text))
                    {
                        throw new KeyNotFoundException(string.Format("Cannot maps {0} to any texture",
                            colorSource[index]));
                    }

                    var deploy = new UnitDeployment();
                    deploy.Deploy(new IsometricCoords(j, i));
                    var sprite = new Sprite(new SingleSpriteSelector(text));
                    sprite.SetValue(IsometricMap.DeploymentProperty, deploy);
                    map.AddChild(sprite);
                }
            }

            return sprites;
        }
    }
}
