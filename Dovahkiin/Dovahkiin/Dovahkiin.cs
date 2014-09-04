using Dovahkiin.Constant;
using Dovahkiin.Screen;
using tranduytrung.Xna.Engine;

namespace Dovahkiin
{
    public class Dovahkiin : GameBase
    {
        public static StartupMenuScreen StartupMenuScreen;
        public static SettingScreen SettingScreen;
        public static ComponentBase GamePlayScreen { get; set; }

        public Dovahkiin()
            : base(1280, 720)
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Fonts.Initialize(Content);
            Sounds.Initialize(Content);
            Textures.Initialize(Content);

            StartupMenuScreen = new StartupMenuScreen(this);
            SettingScreen = new SettingScreen(this);
            GamePlayScreen = new GamePlayScreen(this);
            ChangeScreen(StartupMenuScreen);

            base.Initialize();
        }
    }
}
