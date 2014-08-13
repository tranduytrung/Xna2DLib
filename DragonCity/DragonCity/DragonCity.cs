using tranduytrung.DragonCity.Screen;
using tranduytrung.Xna.Engine;

namespace tranduytrung.DragonCity
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class DragonCity : GameBase
    {
        public static MainMenuScreen MainMenuScreen { get; set; }
        public DragonCity()
        {
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            MainMenuScreen = new MainMenuScreen(this);

            base.Initialize();
        }
    }
}
