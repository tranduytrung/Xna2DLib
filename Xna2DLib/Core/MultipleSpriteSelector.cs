using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace tranduytrung.Xna.Core
{
    public class MultipleSpriteSelector : ISpriteSelector
    {
        private readonly Texture2D[] _textures;
        private TimeSpan _frameRate;
        private TimeSpan _accumulator;
        private int _currentFrame;

        public MultipleSpriteSelector(IEnumerable<Texture2D> textures, double milisecondPerFrame = 50.0)
        {
            FrameRate = TimeSpan.FromMilliseconds(milisecondPerFrame);
            _textures = textures.ToArray();
        }

        public MultipleSpriteSelector(IEnumerable<Texture2D> textures, TimeSpan frameRate)
        {
            FrameRate = frameRate;
            _textures = textures.ToArray();
        }

        public Texture2D[] Textures
        {
            get { return _textures; }
        }

        public TimeSpan FrameRate
        {
            get { return _frameRate; }
            set { _frameRate = value; }
        }

        public SpriteSelectorState GetFrane(GameTime gameTime, params object[] parameters)
        {
            _accumulator += gameTime.ElapsedGameTime;
            if (_accumulator > FrameRate)
            {
                _currentFrame = (_currentFrame + (int) (_accumulator.Ticks/_frameRate.Ticks)) % _textures.Length;
                _accumulator = TimeSpan.FromTicks(_accumulator.Ticks % _frameRate.Ticks);
            }

            return new MultipleSpriteSelectorState(_textures[_currentFrame], _textures[_currentFrame].Bounds, _currentFrame);
        }
    }
}
