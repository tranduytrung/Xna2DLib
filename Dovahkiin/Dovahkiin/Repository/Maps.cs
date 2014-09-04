using System;
using System.Collections.Generic;
using Dovahkiin.Constant;
using Dovahkiin.Model.TileModel;
using Dovahkiin.Utility;
using Microsoft.Xna.Framework;
using tranduytrung.Xna.Map;

namespace Dovahkiin.Repository
{
    public static class Maps
    {
        public static IsometricMap GetMap(MapName mapName)
        {
            switch (mapName)
            {
                case MapName.Sagaland:
                    var sagaland = ColorTerrainBuilder.CreateMap(Textures.MapSagaland,
                        new Dictionary<Color, TileMappingInfo>()
                        {
                            {
                                new Color(0, 255, 0),
                                new TileMappingInfo() {Model = new Tile("grass", true), Texture = Textures.TileGrass}
                            },
                            {
                                new Color(0, 128, 0),
                                new TileMappingInfo() {Model = new Tile("lush", true), Texture = Textures.TileLush}
                            },
                            {
                                new Color(255, 178, 127),
                                new TileMappingInfo() {Model = new Tile("desert", true), Texture = Textures.TileDesert}
                            },
                            {
                                new Color(0, 0, 255),
                                new TileMappingInfo()
                                {
                                    Model = new Tile("cryogentic", true),
                                    Texture = Textures.TileCryogenic
                                }
                            },
                        }, Textures.MapColorKey);

                    return sagaland;
                default:
                    throw new ArgumentOutOfRangeException("mapName");
            }
        }
    }

    public enum MapName
    {
        Sagaland
    }
}
