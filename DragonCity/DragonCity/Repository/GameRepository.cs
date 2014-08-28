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
            var buildingShop = new BuildingShop();
            buildingShop.ApplyData(map, GetBuildings());
            yield return buildingShop;

            var dragonShop = new DragonShop();
            dragonShop.ApplyData(map, GetDragons());
            yield return dragonShop;

            yield return new InGameMenu();
        }

        public static IEnumerable<Building> GetBuildings()
        {
            yield return new Farm();
        }

        public static IEnumerable<DragonBase> GetDragons()
        {
            yield return new Dragon();
        }
    }
}
