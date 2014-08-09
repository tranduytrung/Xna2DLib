using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace tranduytrung.Xna.Core
{
    public struct Size
    {
        public int Width;
        public int Height;

        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public static implicit operator Point(Size size)
        {
            return new Point(size.Width, size.Height);
        }
    }
}
