using Microsoft.Xna.Framework;
using tranduytrung.Xna.Core;

namespace tranduytrung.Xna.Engine
{
    public class GameBase : Game
    {
        public ComponentBase ActiveScreen { get; private set; }

        protected override void Initialize()
        {
            GlobalGameState.GraphicsDevice = GraphicsDevice;
            GlobalGameState.GameInstance = this;
            base.Initialize();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Input.Update();
            AnimationManager.Update();
            GlobalGameState.GameTime = gameTime;

            base.Update(gameTime);
        }

        public void TransitTo(ComponentBase screen, bool removeOldScreen)
        {
            if (removeOldScreen && ActiveScreen != null)
            {
                Components.Remove(ActiveScreen);
            }

            screen.Enabled = true;
            screen.Visible = true;
            Components.Add(screen);
        }

        public void Remove(ComponentBase screen)
        {
            Components.Remove(screen);
        }
    }
}
