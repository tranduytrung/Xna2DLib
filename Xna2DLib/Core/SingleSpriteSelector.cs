using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace tranduytrung.Xna.Core
{
    public class SingleSpriteSelector : ISpriteSelector
    {
        private readonly Texture2D _texture;
        private readonly SpriteSelectorState _state;

        public SingleSpriteSelector(Texture2D texture)
        {
            _texture = texture;
            _state = new SpriteSelectorState(Texture, new Rectangle(0,0, _texture.Width, _texture.Height)); 
        }

        public Texture2D Texture
        {
            get { return _texture; }
        }

        public SpriteSelectorState GetFrane(GameTime gameTime, params object[] parameters)
        {
            return _state;
        }
    }
}
