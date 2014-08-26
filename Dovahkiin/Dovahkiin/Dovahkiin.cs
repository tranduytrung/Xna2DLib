using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using tranduytrung.Xna.Engine;

namespace Dovahkiin
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Dovahkiin : GameBase
    {
        public Dovahkiin()
            : base(1280, 720)
        {
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
    }
}
