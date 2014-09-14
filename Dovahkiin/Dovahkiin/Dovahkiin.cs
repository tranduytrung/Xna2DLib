using Dovahkiin.Constant;
using Dovahkiin.Repository;
using Dovahkiin.Screen;
using tranduytrung.Xna.Engine;

namespace Dovahkiin
{
    public class Dovahkiin : GameBase
    {
        public static StartupMenuScreen StartupMenuScreen { get; private set; }
        public static SettingScreen SettingScreen { get; private set; }
        public static GamePlayScreen GamePlayScreen { get; private set; }
        public static InventoryScreen InventoryScreen { get; private set; }
        public static ActionSuggestionScreen ActionSuggestionScreen { get; private set; }
        public static TradingScreen TradingScreen { get; private set; }
        public static QuickBattleScreen QuickBattleScreen { get; private set; }

        public Dovahkiin()
            : base(1280, 720)
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Resouces.Initialize(Content);
            Fonts.Initialize();
            Textures.Initialize();
            Sounds.Initialize(Content);

            StartupMenuScreen = new StartupMenuScreen(this);
            SettingScreen = new SettingScreen(this);
            GamePlayScreen = new GamePlayScreen(this);
            InventoryScreen = new InventoryScreen(this);
            ActionSuggestionScreen = new ActionSuggestionScreen(this);
            TradingScreen = new TradingScreen(this);
            QuickBattleScreen = new QuickBattleScreen(this);

            ChangeScreen(StartupMenuScreen);
            EagerScreen(SettingScreen);
            EagerScreen(ActionSuggestionScreen);
            EagerScreen(QuickBattleScreen);

            base.Initialize();
        }

        public void NewGamePlayScreen()
        {
            GamePlayScreen = new GamePlayScreen(this);
        }
    }
}
