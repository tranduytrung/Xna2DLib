using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace tranduytrung.Xna.Core
{
    public static class Input
    {
        private static MouseState _previousMouseState;
        private static int _enableGamePad;

        [DefaultValue(true)]
        public static bool EnableMouse { get; set; }

        [DefaultValue(true)]
        public static bool EnableKeyboard { get; set; }

        [DefaultValue(1)]
        public static int EnableGamePad
        {
            get { return _enableGamePad; }
            set
            {
                if (value < 0 || value > 4)
                {
                    throw new ArgumentException("value out of 0 to 4");
                }

                _enableGamePad = value;
            }
        }

        public static MouseState MouseState { get; set; }

        public static KeyboardState KeyboardState { get; set; }

        public static GamePadState[] GamePadStates { get; set; }

        static Input()
        {
            EnableMouse = EnableKeyboard = true;
            EnableGamePad = 1;
            GamePadStates = new GamePadState[4];
        }

        internal static void Update()
        {
            if (EnableMouse)
            {
                _previousMouseState = MouseState;
                MouseState = Mouse.GetState();
            }

            if (EnableKeyboard)
            {
                KeyboardState = Keyboard.GetState();
            }

            for (int i = 0; i < EnableGamePad; ++i)
            {
                GamePadStates[i] = GamePad.GetState((PlayerIndex) i);
            }
        }

        public static bool IsLeftMouseButtonDown()
        {
            return _previousMouseState.LeftButton == ButtonState.Released && MouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool IsLeftMouseButtonUp()
        {
            return _previousMouseState.LeftButton == ButtonState.Pressed && MouseState.LeftButton == ButtonState.Released;
        }

        public static bool IsLeftMousePressed()
        {
            return _previousMouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool IsRightMouseButtonDown()
        {
            return _previousMouseState.RightButton == ButtonState.Released && MouseState.RightButton == ButtonState.Pressed;
        }

        public static bool IsRightMouseButtonUp()
        {
            return _previousMouseState.RightButton == ButtonState.Pressed && MouseState.RightButton == ButtonState.Released;
        }

        public static bool IsRightMousePressed()
        {
            return _previousMouseState.RightButton == ButtonState.Pressed;
        }

        public static bool IsMouseMoved()
        {
            return _previousMouseState.X != MouseState.X || _previousMouseState.Y != MouseState.Y;
        }

        public static Vector2 MouseOffset()
        {
            return new Vector2(MouseState.X - _previousMouseState.X, MouseState.Y - _previousMouseState.Y);
        }
    }
}
