using Microsoft.Xna.Framework.Input;
using System;

namespace tranduytrung.Xna.Core
{
    public class MouseEventArgs : EventArgs
    {
        public MouseEventArgs()
        {
        }

        public MouseState MouseState
        {
            get { return Input.MouseState; }
        }
    }
}
