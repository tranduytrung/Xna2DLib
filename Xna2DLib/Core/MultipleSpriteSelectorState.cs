using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace tranduytrung.Xna.Core
{
    public class MultipleSpriteSelectorState : SpriteSelectorState
    {
        private readonly int _index;
        public MultipleSpriteSelectorState(Texture2D texture, Rectangle clipBounds, int index) : base(texture, clipBounds)
        {
            _index = index;
        }

        public int Index
        {
            get { return _index; }
        }
    }
}
