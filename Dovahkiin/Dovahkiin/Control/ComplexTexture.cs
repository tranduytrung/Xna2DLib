using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Dovahkiin.Control
{
    public class ComplexTexture
    {
        private readonly Dictionary<State, Dictionary<Direction, Texture2D[]>> _texturesDict =
            new Dictionary<State, Dictionary<Direction, Texture2D[]>>();

        public Texture2D[] GetTextures(State state, Direction direction)
        {
            return _texturesDict[state][direction];
        }

        public void AddTextures(State state, Direction direction,  Texture2D[] textures)
        {
            if (!_texturesDict.ContainsKey(state))
                _texturesDict[state] = new Dictionary<Direction, Texture2D[]>();

            _texturesDict[state][direction] = textures;
        }
    }

    public enum State { stopped, walking }
    public enum Direction { n, ne, e, se, s, sw, w, nw };
}
