using Microsoft.Xna.Framework.Input;
using System;

namespace tranduytrung.Xna.Core
{
    public class MouseEventArgs : EventArgs
    {
        public MouseEventArgs()
        {
        }

        public MouseEventArgs(int x, int y)
        {
            Y = y;
            X = x;
        }

        public int X { get; private set; }
        public int Y { get; private set; }

        public MouseState MouseState
        {
            get { return Input.MouseState; }
        }
    }
}
