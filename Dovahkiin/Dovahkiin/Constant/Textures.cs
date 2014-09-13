using Dovahkiin.Control;
using Dovahkiin.Repository;
using System.Collections.Generic;

namespace Dovahkiin.Constant
{
    public static class Textures
    {
        public static int MainMenuLogo;
        public static int SettingMenuLogo;
        public static int MainMenuBackground;

        public static int ButtonNormal;
        public static int ButtonHover;
        public static int ButtonPressed;
        
        public static int MenuBox;
        public static int InventoryBox;
        public static int EmptyBox;

        public static int MapCellColorKey;
        public static int SagaLand;

        public static int TileGrass;
        public static int TileLush;
        public static int TileDesert;
        public static int TileCryogenic;

        public static int BloodPotion;
        public static int SmallBloodPotion;
        public static int Coin;

        public static int Poo;
        public static int Knight;
        public static int Barbar;
        public static int ToggleButton;
        public static int ToggleButtonHover;
        public static int ToggleButtonSelected;

        public static int TitleMember;
        public static int TitleItem;
        public static int TitleDescription;
        //public static int Member;
        public static void Initialize()
        {
            // misc
            MainMenuBackground = Resouces.AddTexture(@"images/misc/background");
            MainMenuLogo = Resouces.AddTexture(@"images/misc/logo");
            SettingMenuLogo = Resouces.AddTexture(@"images/misc/settingsLogo");

            // Button and control
            ButtonNormal = Resouces.AddTexture(@"images/button/button-normal");
            ButtonHover = Resouces.AddTexture(@"images/button/button-hover");
            ButtonPressed = Resouces.AddTexture(@"images/button/button-pressed");
            ToggleButton = Resouces.AddTexture(@"images/button/box-normal");
            ToggleButtonHover = Resouces.AddTexture(@"images/button/box-hover");
            ToggleButtonSelected = Resouces.AddTexture(@"images/button/box-selected");

            MenuBox = Resouces.AddTexture(@"images/button/menu-box");
            InventoryBox = Resouces.AddTexture(@"images/button/inventory-box");
            EmptyBox = Resouces.AddTexture(@"images/button/empty-box");

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
            string basePath = "images/creature/human/knight";
            var dictPath = new Dictionary<State, Dictionary<Direction, string[]>>();
            dictPath[State.walking] = CompletePath(basePath, State.walking, " ", 12);
            dictPath[State.stopped] = CompletePath(basePath, State.stopped, " ", 1);
            Knight = Resouces.AddComplexTexture(dictPath);

            // Barbar
            basePath = "images/creature/human/barbar";
            dictPath = new Dictionary<State, Dictionary<Direction, string[]>>();
            dictPath[State.walking] = CompletePath(basePath, State.walking, " ", 8);
            dictPath[State.stopped] = CompletePath(basePath, State.stopped, " ", 1);
            Barbar = Resouces.AddComplexTexture(dictPath);

            // Items
            BloodPotion = Resouces.AddTexture(@"images/item/BloodPotion");
            SmallBloodPotion = Resouces.AddTexture(@"images/item/SmallBloodPotion");
            Coin = Resouces.AddTexture(@"images/item/Coin");

            // Titles
            TitleMember = Resouces.AddTexture(@"images/misc/memberTitle");
            TitleItem = Resouces.AddTexture(@"images/misc/itemTitle");
            TitleDescription = Resouces.AddTexture(@"images/misc/descriptionTitle");

            // Testing zone
            //Member = Resouces.AddTexture(@"images/creature/human/knight/stopped se0000");
        }

        private static Dictionary<Direction, string[]> CompletePath(string basePath, State state, string seperator, int max)
        {
            var dict = new Dictionary<Direction, string[]>();
            dict[Direction.n] = new string[max];
            for (int i = 0; i < max; ++i)
            {
                dict[Direction.n][i] = basePath + "/" + state + seperator + "n" + i.ToString("0000");
            }

            dict[Direction.ne] = new string[max];
            for (int i = 0; i < max; ++i)
            {
                dict[Direction.ne][i] = basePath + "/" + state + seperator + "ne" + i.ToString("0000");
            }

            dict[Direction.e] = new string[max];
            for (int i = 0; i < max; ++i)
            {
                dict[Direction.e][i] = basePath + "/" + state + seperator + "e" + i.ToString("0000");
            }

            dict[Direction.se] = new string[max];
            for (int i = 0; i < max; ++i)
            {
                dict[Direction.se][i] = basePath + "/" + state + seperator + "se" + i.ToString("0000");
            }

            dict[Direction.s] = new string[max];
            for (int i = 0; i < max; ++i)
            {
                dict[Direction.s][i] = basePath + "/" + state + seperator + "s" + i.ToString("0000");
            }

            dict[Direction.sw] = new string[max];
            for (int i = 0; i < max; ++i)
            {
                dict[Direction.sw][i] = basePath + "/" + state + seperator + "sw" + i.ToString("0000");
            }

            dict[Direction.w] = new string[max];
            for (int i = 0; i < max; ++i)
            {
                dict[Direction.w][i] = basePath + "/" + state + seperator + "w" + i.ToString("0000");
            }

            dict[Direction.nw] = new string[max];
            for (int i = 0; i < max; ++i)
            {
                dict[Direction.nw][i] = basePath + "/" + state + seperator + "nw" + i.ToString("0000");
            }

            return dict;
        }
    }
}
