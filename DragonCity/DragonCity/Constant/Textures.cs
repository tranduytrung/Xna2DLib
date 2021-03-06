﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace tranduytrung.DragonCity.Constant
{
    public static class Textures
    {
        public static Texture2D MainMenuBackground;
        public static Texture2D MainMenuLogo;
        public static Texture2D Setting;

        public static Texture2D ButtonNormal;
        public static Texture2D ButtonHover;
        public static Texture2D ButtonPressed;
        public static Texture2D ToggleButtonSelected;
        public static Texture2D ToggleButtonNormal;
        public static Texture2D ToggleButtonHover;

        public static Texture2D TileGrass;
        public static Texture2D TileLush;
        public static Texture2D TileDesert;
        public static Texture2D TileCryogenic;
        public static Texture2D MapColorKey;

        public static Texture2D MapSagaland;

        public static Texture2D Farm;
        public static Texture2D Dragonarium;

        public static Texture2D Poo;

        public static Texture2D Tomato;
        public static Texture2D Gold;
        public static Texture2D Terra;

        public static void LoadContent(ContentManager content)
        {
            // misc
            MainMenuBackground = content.Load<Texture2D>(@"images/misc/background");
            MainMenuLogo = content.Load<Texture2D>(@"images/misc/logo");
            Setting = content.Load<Texture2D>(@"images/misc/setting");
            Gold = content.Load<Texture2D>(@"images/misc/gold");

            // Button and control
            ButtonNormal = content.Load<Texture2D>(@"images/button/button-normal");
            ButtonHover = content.Load<Texture2D>(@"images/button/button-hover");
            ButtonPressed = content.Load<Texture2D>(@"images/button/button-pressed");
            ToggleButtonNormal = content.Load<Texture2D>(@"images/button/box-normal");
            ToggleButtonSelected= content.Load<Texture2D>(@"images/button/box-selected");
            ToggleButtonHover = content.Load<Texture2D>(@"images/button/box-hover");

            // map and tile
            MapColorKey = content.Load<Texture2D>(@"images/terrain/colorKey128x64");
            TileGrass= content.Load<Texture2D>(@"images/terrain/grass");
            TileLush= content.Load<Texture2D>(@"images/terrain/lush");
            TileDesert = content.Load<Texture2D>(@"images/terrain/desert");
            TileCryogenic = content.Load<Texture2D>(@"images/terrain/cryogenic");
            MapSagaland= content.Load<Texture2D>(@"images/maps/sagaland");

            // building
            Farm = content.Load<Texture2D>(@"images/buildings/food-farm");
            Dragonarium = content.Load<Texture2D>(@"images/buildings/dragonarium");
            Terra = content.Load<Texture2D>(@"images/buildings/terra");

            // dragon
            Poo = content.Load<Texture2D>(@"images/dragon/poo");

            // food
            Tomato = content.Load<Texture2D>(@"images/food/tomato");
        }
    }
}
