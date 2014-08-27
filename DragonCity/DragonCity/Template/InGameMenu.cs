using tranduytrung.DragonCity.Constant;
using tranduytrung.DragonCity.Model;
using tranduytrung.DragonCity.Utility;
using tranduytrung.Xna.Control;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Engine;
using tranduytrung.Xna.Map;

namespace tranduytrung.DragonCity.Template
{
    public class InGameMenu : ITemplate
    {
        public DrawableObject PresentableContent { get; private set; }
        public DrawableObject ContextMenu { get; private set; }
        public void Start()
        {
        }

        public void End()
        {
        }

        public void ApplyData(IsometricMap map, object data)
        {
        }

        public InGameMenu()
        {
            PresentableContent = new Sprite(new SingleSpriteSelector(Textures.Setting)) {SpriteMode = SpriteMode.Fit};
            InitContextMenu();
        }

        private void InitContextMenu()
        {
            var panel = new StackPanel();
            panel.Height = ControlConfig.ButtonHeight + 24;
            panel.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Center);

            var backButton = ControlFactory.CreateButton("back", Fonts.ButtonFont, Textures.ButtonNormal,
                Textures.ButtonHover, Textures.ButtonPressed);
            backButton.Margin = new Margin(24, 12);
            backButton.Click += BackToMainMenu;
            panel.Children.Add(backButton);

            var quitButton = ControlFactory.CreateButton("quit", Fonts.ButtonFont, Textures.ButtonNormal,
                Textures.ButtonHover, Textures.ButtonPressed);
            quitButton.Margin = new Margin(24, 12);
            quitButton.Click += QuitGame;
            panel.Children.Add(quitButton);

            ContextMenu = panel;
        }

        private static void QuitGame(object sender, MouseEventArgs e)
        {
            GameContext.GameInstance.Exit();
        }

        private static void BackToMainMenu(object sender, MouseEventArgs e)
        {
            GameContext.GameInstance.ChangeScreen(DragonCity.MainMenuScreen);
        }
    }
}
