﻿using System.Linq;
using Dovahkiin.Model.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Map;
using Dovahkiin.Extension;

namespace Dovahkiin.Utility
{
    public class TileMappingInfo
    {
        public Texture2D Texture { get; set; }
        public IMapObject Model { get; set; }
    }

    public static class ColorTerrainBuilder
    {
        /// <summary>
        /// Build the terrain base on color of texture
        /// </summary>
        /// <param name="source">the texture typed Texture2D</param>
        /// <param name="mappingDictionary">the mapping rules</param>
        /// <param name="colorKeyTexture">color key texture</param>
        /// <returns>collection of sprite in dictionary order</returns>
        public static IsometricMap CreateMap(Texture2D source, IDictionary<Color, TileMappingInfo> mappingDictionary, Texture2D colorKeyTexture)
        {
            var protoTile = mappingDictionary.First().Value;
            var colorKey = new Color[colorKeyTexture.Width*colorKeyTexture.Height];
            colorKeyTexture.GetData(colorKey);

            var map = new IsometricMap(source.Height, source.Width, protoTile.Texture.Width, protoTile.Texture.Height,
                colorKey);

            var colorSource = new Color[source.Width * source.Height];
            source.GetData(colorSource);

            for (var i = 0; i < source.Height; ++i)
            {
                for (var j = 0; j < source.Width; ++j)
                {
                    if (i % 2 != j % 2)
                        continue;

                    var index = source.Width * i + j;

                    TileMappingInfo info;
                    if (!mappingDictionary.TryGetValue(colorSource[index], out info))
                    {
                        throw new KeyNotFoundException(string.Format("Cannot maps {0} to any texture",
                            colorSource[index]));
                    }

                    var deploy = new UnitDeployment();
                    deploy.Deploy(new IsometricCoords(j, i));
                    var sprite = new Sprite(new SingleSpriteSelector(info.Texture));
                    sprite.SetModel(info.Model);
                    sprite.SetValue(IsometricMap.DeploymentProperty, deploy);
                    map.SetTile(sprite);
                }
            }

            return map;
        }
    }

}
