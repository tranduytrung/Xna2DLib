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
        public DragonCity() : base(1280, 800)
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            MainMenuScreen = new MainMenuScreen(this);
            ChangeScreen(MainMenuScreen);

            base.Initialize();
        }
    }
}
