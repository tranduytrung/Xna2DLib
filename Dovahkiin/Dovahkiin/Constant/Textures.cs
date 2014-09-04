using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace Dovahkiin.Constant
{
    public static class Textures
    {
        public static Texture2D MainMenuBackground;

        public static Texture2D ButtonNormal;
        public static Texture2D ButtonHover;
        public static Texture2D ButtonPressed;
        public static void Initialize()
        {
            
        }
        public static void LoadContent(ContentManager content)
        {
            // misc
            MainMenuBackground = content.Load<Texture2D>(@"images/misc/background");

            // Button and control
            //ButtonNormal = content.Load<Texture2D>(@"images/button/button-normal");
            //ButtonHover = content.Load<Texture2D>(@"images/button/button-hover");
            //ButtonPressed = content.Load<Texture2D>(@"images/button/button-pressed");
        }
    }
}
