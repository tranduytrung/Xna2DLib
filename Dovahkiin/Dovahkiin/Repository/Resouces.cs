using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Dovahkiin.Repository
{
    public static class Resouces
    {
        private static int _autoNumber = 0;
        private static ContentManager _content;

        private static readonly Dictionary<int, object> ResouceDictionary = new Dictionary<int, object>();

        public static void Initialize(ContentManager content)
        {
            _content = content;
        }

        public static int AddTexture(Texture2D texture)
        {
            var key = _autoNumber++;
            ResouceDictionary.Add(key, texture);
            return key;
        }

        public static int AddTexture(string texturePath)
        {
            return AddTexture(_content.Load<Texture2D>(texturePath));
        }

        public static int AddFont(SpriteFont font)
        {
            var key = _autoNumber++;
            ResouceDictionary.Add(key, font);
            return key;
        }

        public static int AddFont(string texturePath)
        {
            return AddFont(_content.Load<SpriteFont>(texturePath));
        }

        public static Texture2D GetTexture(int id)
        {
            return (Texture2D) ResouceDictionary[id];
        }

        public static SpriteFont GetFont(int id)
        {
            return (SpriteFont)ResouceDictionary[id];
        }

        public static void Dispose()
        {
            foreach (var item in ResouceDictionary.Values)
            {
                var texture = item as Texture2D;
                if (texture != null)
                {
                    texture.Dispose();
                }
            }
        }
    }
}
