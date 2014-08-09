using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace tranduytrung.Xna.Core
{
    public class SpriteSelectorState
    {
        public SpriteSelectorState(Texture2D texture, Rectangle clipBounds)
        {
            Texture = texture;
            ClipBounds = clipBounds;
        }

        public Texture2D Texture { get; set; }

        public Rectangle ClipBounds { get; set; }
    }
}
