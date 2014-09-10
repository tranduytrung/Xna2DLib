using Dovahkiin.Control;
using Dovahkiin.Repository;
using System.Collections.Generic;

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
        public static int Knight;
        public static int ToggleButton;
        public static int ToggleButtonHover;
        public static int ToggleButtonSelected;

        public static void Initialize()
        {
            // misc
            MainMenuBackground = Resouces.AddTexture(@"images/misc/background");
            MainMenuLogo = Resouces.AddTexture(@"images/misc/logo");

            // Button and control
            ButtonNormal = Resouces.AddTexture(@"images/button/button-normal");
            ButtonHover = Resouces.AddTexture(@"images/button/button-hover");
            ButtonPressed = Resouces.AddTexture(@"images/button/button-pressed");
            ToggleButton = Resouces.AddTexture(@"images/button/box-normal");
            ToggleButtonHover = Resouces.AddTexture(@"images/button/box-hover");
            ToggleButtonSelected = Resouces.AddTexture(@"images/button/box-selected");

            // map and tile
            MapCellColorKey = Resouces.AddTexture(@"images/terrain/colorKey128x64");
            TileGrass = Resouces.AddTexture(@"images/terrain/grass");
            TileLush = Resouces.AddTexture(@"images/terrain/lush");
            TileDesert = Resouces.AddTexture(@"images/terrain/desert");
            TileCryogenic = Resouces.AddTexture(@"images/terrain/cryogenic");
            SagaLand = Resouces.AddTexture(@"images/maps/sagaland");

            // creature
            Poo = Resouces.AddTexture(@"images/creature/human/poo");
            
            // knight
            ComplexTexture compTexture = new ComplexTexture();
            Dictionary<State, string[]> statePathsDict = new Dictionary<State, string[]>();
            Dictionary<State, int> stateCountDict = new Dictionary<State, int>();
            Dictionary<State, string[]> pathsDict = new Dictionary<State, string[]>();
            string basePath = "images/creature/human/knight";
                // walking
                State state = State.walking;
                int count = 12;
                string[] knightPaths = new string[count * 8];
                stateCountDict.Add(state, count);

                completePath(ref knightPaths, basePath, state.ToString(), " ", count);
                statePathsDict.Add(state, knightPaths);

                pathsDict.Add(state, knightPaths);
                compTexture.CountDict.Add(state, count);
            Knight = Resouces.AddComplexTexture(compTexture, pathsDict);
        }

        private static void completePath(ref string[] array, string basePath, string state, string seperator, int max)
        {
            int index = 0;
            for (int i = 0; i < max; ++i)
            {
                array[index] = basePath + "/" + state + seperator + "n" + i.ToString("0000");
                ++index;
            }
            for (int i = 0; i < max; ++i)
            {
                array[index] = basePath + "/" + state + seperator + "ne" + i.ToString("0000");
                ++index;
            }
            for (int i = 0; i < max; ++i)
            {
                array[index] = basePath + "/" + state + seperator + "e" + i.ToString("0000");
                ++index;
            }
            for (int i = 0; i < max; ++i)
            {
                array[index] = basePath + "/" + state + seperator + "se" + i.ToString("0000");
                ++index;
            }
            for (int i = 0; i < max; ++i)
            {
                array[index] = basePath + "/" + state + seperator + "s" + i.ToString("0000");
                ++index;
            }
            for (int i = 0; i < max; ++i)
            {
                array[index] = basePath + "/" + state + seperator + "sw" + i.ToString("0000");
                ++index;
            }
            for (int i = 0; i < max; ++i)
            {
                array[index] = basePath + "/" + state + seperator + "w" + i.ToString("0000");
                ++index;
            }
            for (int i = 0; i < max; ++i)
            {
                array[index] = basePath + "/" + state + seperator + "nw" + i.ToString("0000");
                ++index;
            }
        }
    }
}
