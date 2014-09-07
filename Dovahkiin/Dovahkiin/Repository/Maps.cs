using System;
using System.Collections.Generic;
using Dovahkiin.Constant;
using Dovahkiin.Maps;
using Dovahkiin.Model.TileModel;
using Dovahkiin.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tranduytrung.Xna.Map;

namespace Dovahkiin.Repository
{
    public static class Maps
    {
        public const string SagaLand = "saga-land";

        public static Map GetModel(string mapName)
        {
            switch (mapName)
            {
                case SagaLand:
                    return ColorMap.CreateMap(SagaLand, Textures.SagaLand, new Dictionary<Color, Tile>()
                    {
                        {new Color(0, 255, 0), new Tile(TileName.Grass, true)},
                        {new Color(0, 128, 0),new Tile(TileName.Lush, true)},
                        {new Color(255, 178, 127), new Tile(TileName.Desert, true)},
                        {new Color(0, 0, 255), new Tile(TileName.Cryogentic, true)}
                    });
                default:
                    throw new ArgumentOutOfRangeException("mapName");
            }
        }

        public static IsometricMap GetControl(Map mapModel)
        {
            switch (mapModel.Name)
            {
                case SagaLand:
                    return ColorTerrainBuilder.CreateMap(mapModel, Textures.MapCellColorKey, new Dictionary<string, Texture2D>()
                    {
                        {TileName.Grass, Textures.TileGrass},
                        {TileName.Lush, Textures.TileLush},
                        {TileName.Desert, Textures.TileDesert},
                        {TileName.Cryogentic, Textures.TileCryogenic}
                    });
                default:
                    throw new ArgumentOutOfRangeException("mapModel");
            }
        }
    }
}
