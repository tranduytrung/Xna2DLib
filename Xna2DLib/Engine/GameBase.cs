using System.Collections.Generic;
using Microsoft.Xna.Framework;
using tranduytrung.Xna.Core;

namespace tranduytrung.Xna.Engine
{
    public class GameBase : Game
    {
        private GraphicsDeviceManager graphics;
        internal readonly HashSet<Timer> GlobalTimerList = new HashSet<Timer>();
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
            UpdateTimer();

            base.Update(gameTime);
        }

        /// <summary>
        /// Eagerly load and initialize screen but not show it up
        /// </summary>
        /// <param name="screen">the screen</param>
        public void EagerScreen(ComponentBase screen)
        {
            screen.Enabled = false;
            screen.Visible = false;

            if (!Components.Contains(screen))
                Components.Add(screen);
        }

        /// <summary>
        /// Change to specific screen
        /// </summary>
        /// <param name="screen">the screen to show up</param>
        /// <param name="removeOldScreen">wether or not deactived screen is removed from scren pool</param>
        public void ChangeScreen(ComponentBase screen, bool removeOldScreen = false)
        {
            if (ActiveScreen != null)
            {
                ActiveScreen.OnTransitTo();
                ActiveScreen.Enabled = false;
                ActiveScreen.Visible = false;
                if (removeOldScreen)
                {
                    Components.Remove(ActiveScreen);
                }
            }
            
            screen.Enabled = true;
            screen.Visible = true;
            ActiveScreen = screen;
            if (!Components.Contains(screen))
                Components.Add(screen);
            ActiveScreen.OnTransitFrom();
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

        private void UpdateTimer()
        {
            GlobalTimerList.RemoveWhere(timer => !timer.Update(GameContext.GameTime.ElapsedGameTime));
        }
    }
}
