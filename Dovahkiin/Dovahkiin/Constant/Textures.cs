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

        public static Texture2D MapCellColorKey;
        public static Texture2D SagaLand;

        public static Texture2D TileGrass;
        public static Texture2D TileLush;
        public static Texture2D TileDesert;
        public static Texture2D TileCryogenic;
        public static void Initialize(ContentManager content)
        {
            // misc
            MainMenuBackground = content.Load<Texture2D>(@"images/misc/background");

            // Button and control
            ButtonNormal = content.Load<Texture2D>(@"images/button/button-normal");
            ButtonHover = content.Load<Texture2D>(@"images/button/button-hover");
            ButtonPressed = content.Load<Texture2D>(@"images/button/button-pressed");

            // map and tile
            MapCellColorKey = content.Load<Texture2D>(@"images/terrain/colorKey128x64");
            TileGrass = content.Load<Texture2D>(@"images/terrain/grass");
            TileLush = content.Load<Texture2D>(@"images/terrain/lush");
            TileDesert = content.Load<Texture2D>(@"images/terrain/desert");
            TileCryogenic = content.Load<Texture2D>(@"images/terrain/cryogenic");
            SagaLand = content.Load<Texture2D>(@"images/maps/sagaland");
        }
    }
}
