using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace tranduytrung.Xna.Engine
{
    public static class GameContext
    {
        public static GraphicsDevice GraphicsDevice { get; internal set; }
        public static GameTime GameTime { get; internal set; }
        public static GameBase GameInstance { get; internal set; }
    }
}
