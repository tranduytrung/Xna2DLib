using System;
using System.Collections.Generic;
using Dovahkiin.Constant;
using Dovahkiin.Control;
using Dovahkiin.Maps;
using Dovahkiin.Model.TileModel;
using Dovahkiin.Utility;
using Microsoft.Xna.Framework;
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
                    return ColorMap.CreateMap(SagaLand, Resouces.GetTexture(Textures.SagaLand), new Dictionary<Color, Tile>()
                    {
                        {new Color(0, 255, 0), new Tile(Textures.TileGrass, TileName.Grass, true)},
                        {new Color(0, 128, 0),new Tile(Textures.TileLush, TileName.Lush, true)},
                        {new Color(255, 178, 127), new Tile(Textures.TileDesert, TileName.Desert, true)},
                        {new Color(0, 0, 255), new Tile(Textures.TileCryogenic, TileName.Cryogentic, true)}
                    });
                default:
                    throw new ArgumentOutOfRangeException("mapName");
            }
        }

        public static HybridMap GetControl(Map mapModel)
        {
            switch (mapModel.Name)
            {
                case SagaLand:
                    return ColorTerrainBuilder.CreateMap(mapModel, Resouces.GetTexture(Textures.MapCellColorKey));
                default:
                    throw new ArgumentOutOfRangeException("mapModel");
            }
        }
    }
}
