using Dovahkiin.Repository;

namespace Dovahkiin.Constant
{
    public static class Textures
    {
        public static int MainMenuLogo;
        public static int MainMenuBackground;

        public static int ButtonNormal;
        public static int ButtonHover;
        public static int ButtonPressed;

        public static int MapCellColorKey;
        public static int SagaLand;

        public static int TileGrass;
        public static int TileLush;
        public static int TileDesert;
        public static int TileCryogenic;

        public static int Poo;

        public static void Initialize()
        {
            // misc
            MainMenuBackground = Resouces.AddTexture(@"images/misc/background");
            MainMenuLogo = Resouces.AddTexture(@"images/misc/logo");

            // Button and control
            ButtonNormal = Resouces.AddTexture(@"images/button/button-normal");
            ButtonHover = Resouces.AddTexture(@"images/button/button-hover");
            ButtonPressed = Resouces.AddTexture(@"images/button/button-pressed");

            // map and tile
            MapCellColorKey = Resouces.AddTexture(@"images/terrain/colorKey128x64");
            TileGrass = Resouces.AddTexture(@"images/terrain/grass");
            TileLush = Resouces.AddTexture(@"images/terrain/lush");
            TileDesert = Resouces.AddTexture(@"images/terrain/desert");
            TileCryogenic = Resouces.AddTexture(@"images/terrain/cryogenic");
            SagaLand = Resouces.AddTexture(@"images/maps/sagaland");

            // creature
            Poo = Resouces.AddTexture(@"images/creature/human/poo");
        }
    }
}
