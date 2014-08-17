using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TestGame;
using tranduytrung.Xna.Engine;

namespace Assignment_2
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : GameBase
    {
        GraphicsDeviceManager graphics;
        private MainMenuComponent _mainMenu;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _mainMenu = new MainMenuComponent(this);
            Components.Add(_mainMenu);
            IsMouseVisible = true;
            GraphicsDevice.DepthStencilState = DepthStencilState.None;

            base.Initialize();
        }
    }
}
