using System.Collections.Generic;
using tranduytrung.DragonCity.Model;
using tranduytrung.DragonCity.Template;
using tranduytrung.Xna.Map;

namespace tranduytrung.DragonCity.Repository
{
    public static class GameRepository
    {
        public static IEnumerable<ITemplate> GetGamePlayServices(IsometricMap map)
        {
            yield return new InGameMenu();
            var buildingShop = new BuildingShop();
            buildingShop.ApplyData(map, GetBuildings());
            yield return buildingShop;
        }

        public static IEnumerable<Building> GetBuildings()
        {
            yield return new Farm();
        }
    }
}
