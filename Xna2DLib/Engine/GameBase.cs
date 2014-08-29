using Microsoft.Xna.Framework;
using tranduytrung.Xna.Core;

namespace tranduytrung.Xna.Engine
{
    public class GameBase : Game
    {
        private GraphicsDeviceManager graphics;
        public ComponentBase ActiveScreen { get; private set; }

        public GameBase(int width = 800, int height = 600)
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
        }

        protected override void Initialize()
        {
            GameContext.GraphicsDevice = GraphicsDevice;
            GameContext.GameInstance = this;
            base.Initialize();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            GameContext.GameTime = gameTime;
            Input.Update();
            AnimationManager.Update();
            TimerManager.Update();

            base.Update(gameTime);
        }

        public void ChangeScreen(ComponentBase screen, bool removeOldScreen = false)
        {
            if (ActiveScreen != null)
            {
                ActiveScreen.Enabled = false;
                ActiveScreen.Visible = false;
                if (removeOldScreen)
                {
                    Components.Remove(ActiveScreen);
                }
            }
            
            screen.Enabled = true;
            screen.Visible = true;
            if (!Components.Contains(screen))
                Components.Add(screen);

            ActiveScreen = screen;
        }

        public void Remove(ComponentBase screen)
        {
            if (ActiveScreen == screen)
                ActiveScreen = null;

            Components.Remove(screen);
        }

        public void ChangeResolution(int width, int height)
        {
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.ApplyChanges();
        }
    }
}
