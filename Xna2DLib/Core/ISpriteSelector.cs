using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace tranduytrung.Xna.Core
{
    public interface ISpriteSelector
    {
        SpriteSelectorState GetFrane(GameTime gameTime, params object[] parameters);
    }
}
