using Microsoft.Xna.Framework;
using tranduytrung.Xna.Core;

namespace tranduytrung.Xna.Engine
{
    public class GameBase : Game
    {
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
            GlobalGameState.GameTime = gameTime;

            base.Update(gameTime);
        }
    }
}
