using Microsoft.Xna.Framework.Content;
<<<<<<< HEAD
namespace Dovahkiin.Constant
{
    public static class  Fonts
    {
        public static void Initialize(ContentManager Content)
=======
using Microsoft.Xna.Framework.Graphics;
namespace Dovahkiin.Constant
{
    public static class  Fonts
    {
        public static SpriteFont ButtonFont;

        public static void Initialize()
>>>>>>> origin/master
        {

        }
        public static void LoadContent(ContentManager content)
        {
            ButtonFont = content.Load<SpriteFont>(@"fonts/button_font");
        }
    }
}
