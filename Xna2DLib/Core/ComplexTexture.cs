using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tranduytrung.Xna.Core
{
    public class ComplexTexture
    {
        private Dictionary<State, Texture2D[]> texturesDict = new Dictionary<State, Texture2D[]>();

        public Dictionary<State, Texture2D[]> TexturesDict
        {
            get { return texturesDict; }
            set { texturesDict = value; }
        }
        private Dictionary<State, string[]> pathsDict = new Dictionary<State, string[]>();

        public Dictionary<State, string[]> PathsDict
        {
            get { return pathsDict; }
            set { pathsDict = value; }
        }

        /**
         * This dictionary store the number of textures that the sprite at each state at each direction
         * Ex: At state walking, direction nw, the sprite has 8 textures: from "walking nw0001" to "walking nw0007"
         * NOTE: The ORDER of the direction is VERY IMPORTANT, the textures must be inputted in the order n, ne, e, se, s, sw, w, nw.
        */
        private Dictionary<State, int> countDict = new Dictionary<State, int>();

        public Dictionary<State, int> CountDict
        {
            get { return countDict; }
            set { countDict = value; }
        }

        public Texture2D[] GetTextures(State state, Direction direction)
        {
            int count = CountDict[state];
            Texture2D[] textures = new Texture2D[count];
            int from = (int)direction * count;
            for (int i = 0; i < count; ++i)
            {
                textures[i] = TexturesDict[state][from + i];
            }
            return textures;
        }

        public void AddTextures(State state, Texture2D[] textures)
        {
            TexturesDict.Add(state, textures);
        }
    }

    public enum State { stopped, walking }
    public enum Direction { n, ne, e, se, s, sw, w, nw };
}
