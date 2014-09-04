using Dovahkiin.Constant;
using Dovahkiin.Screen;
using tranduytrung.Xna.Engine;

namespace Dovahkiin
{
    public class Dovahkiin : GameBase
    {
        public static StartupMenuScreen StartupMenuScreen;
        public Dovahkiin()
            : base(1280, 720)
        {
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Fonts.Initialize();
            Sounds.Initialize();
            Textures.Initialize();

            Textures.LoadContent(Content);
            StartupMenuScreen = new StartupMenuScreen(this);

            ChangeScreen(StartupMenuScreen);

            base.Initialize();
        }
    }
}
