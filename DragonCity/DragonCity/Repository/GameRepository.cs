using System.Collections.Generic;
using tranduytrung.DragonCity.Model;
using tranduytrung.DragonCity.Template;
using tranduytrung.Xna.Map;

namespace tranduytrung.DragonCity.Repository
{
    public static class GameRepository
    {
        public static IEnumerable<ITemplate> GetGamePlayServices()
        {
            var buildingShop = new BuildingShop();
            buildingShop.ApplyData(GetBuildings());
            yield return buildingShop;

            var dragonShop = new DragonShop();
            dragonShop.ApplyData(GetDragons());
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
