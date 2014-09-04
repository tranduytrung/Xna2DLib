using Dovahkiin.Constant;
using Dovahkiin.Screen;
using tranduytrung.Xna.Engine;

namespace Dovahkiin
{
    public class Dovahkiin : GameBase
    {
        public static StartupMenuScreen StartupMenuScreen { get; private set; }
        public static SettingScreen SettingScreen { get; private set; }
        public static ComponentBase GamePlayScreen { get; private set; }

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
