﻿using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using tranduytrung.Xna.Core;

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

        public static int AddTextures(Texture2D[] textures)
        {
            var key = _autoNumber++;
            ResouceDictionary.Add(key, textures);
            return key;
        }

        public static int AddTextures(string[] texturesPath)
        {
            var textures = new Texture2D[texturesPath.Length];
            for (var index = 0; index < texturesPath.Length; index++)
            {
                textures[index] = _content.Load<Texture2D>(texturesPath[index]);
            }
            return AddTextures(textures);
        }

        //public static int AddTexturesWithState(Dictionary<string, Texture2D[]> textureWithState)
        //{
        //    var key = _autoNumber++;
        //    ResouceDictionary.Add(key, textureWithState);
        //    return key;
        //}

        //public static int AddTexturesWithState(Dictionary<string, string[]> textureAndState)
        //{
        //    Dictionary<string, Texture2D[]> texturesStateDictionary = new Dictionary<string, Texture2D[]>();
        //    foreach (KeyValuePair<string, string[]> entry in textureAndState)
        //    {
        //        string key = entry.Key;
        //        string[] paths = entry.Value;
        //        Texture2D[] textures = new Texture2D[paths.Length];
                
        //        for (int i=0; i<paths.Length; ++i)
        //        {
        //            textures[i] = _content.Load<Texture2D>(paths[i]);
        //        }

        //        texturesStateDictionary.Add(key, textures);
        //    }
        //    return AddTexturesWithState(texturesStateDictionary);
        //}

        public static int AddComplexTexture(ComplexTexture compTexture)
        {
            foreach (KeyValuePair<State, string[]> entry in compTexture.PathsDict)
            {
                State state = entry.Key;
                string[] paths = entry.Value;
                Texture2D[] textures = new Texture2D[paths.Length];

                for (int i = 0; i < paths.Length; ++i)
                {
                    textures[i] = _content.Load<Texture2D>(paths[i]);
                }

                compTexture.TexturesDict.Add(state, textures);
            }

            var key = _autoNumber++;
            ResouceDictionary.Add(key, compTexture);
            return key;
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

        public static Texture2D[] GetTextures(int id)
        {
            return (Texture2D[])ResouceDictionary[id];
        }

        public static ComplexTexture GetComplexTexture(int id)
        {
            return (ComplexTexture)ResouceDictionary[id];
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
