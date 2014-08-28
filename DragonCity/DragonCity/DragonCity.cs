using tranduytrung.DragonCity.Constant;
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
        public static GamePlay GamePlay { get; set; }
        public DragonCity() : base(1280, 720)
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Textures.LoadContent(Content);
            Fonts.LoadContent(Content);

            MainMenuScreen = new MainMenuScreen(this);
            GamePlay = new GamePlay(this);
            ChangeScreen(MainMenuScreen);

            base.Initialize();
        }
    }
}
