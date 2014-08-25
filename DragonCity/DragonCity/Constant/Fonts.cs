using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace tranduytrung.DragonCity.Constant
{
    public static class Fonts
    {
        public static SpriteFont ButtonFont;

        public static void LoadContent(ContentManager content)
        {
            ButtonFont = content.Load<SpriteFont>(@"fonts/button_font");
        }
    }
}
