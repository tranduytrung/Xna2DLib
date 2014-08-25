using System.Collections.Generic;
using tranduytrung.DragonCity.Model;
using tranduytrung.Xna.Map;

namespace tranduytrung.DragonCity.Repository
{
    public static class GameRepository
    {
        public static IEnumerable<IService> GetGamePlayServices(IsometricMap map)
        {
            yield return new InGameMenu();
        }
    }
}
