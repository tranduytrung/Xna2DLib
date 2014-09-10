using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tranduytrung.Xna.Core;

namespace Dovahkiin.Control
{
    public class ComplexMultipleSpriteSelector : ISpriteSelector
    {
        private readonly ComplexTexture _complexTexture;
        private TimeSpan _frameRate;
        private TimeSpan _accumulator;
        private int _currentFrame;
        private State _state;
        private Direction _direction;

        public State State
        {
            get { return _state; }
            set { _state = value; }
        }

        public Direction Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
        

        public ComplexMultipleSpriteSelector(ComplexTexture complexTexture, TimeSpan frameRate, State state, Direction direction)
        {
            _complexTexture = complexTexture;
            FrameRate = frameRate;
            _state = state;
            _direction = direction;
        }

        public ComplexMultipleSpriteSelector(ComplexTexture complexTexture, State state, Direction direction, double milisecondPerFrame = 50.0)
        {
            _complexTexture = complexTexture;
            FrameRate = TimeSpan.FromMilliseconds(milisecondPerFrame);
            _state = state;
            _direction = direction;
        }

        public TimeSpan FrameRate
        {
            get { return _frameRate; }
            set { _frameRate = value; }
        }

        public SpriteSelectorState GetFrane(GameTime gameTime, params object[] parameters)
        {
            Texture2D[] textures = _complexTexture.GetTextures(State, Direction);
            _accumulator += gameTime.ElapsedGameTime;
            if (_accumulator > FrameRate)
            {
                _currentFrame = (_currentFrame + (int)(_accumulator.Ticks / _frameRate.Ticks)) % textures.Length;
                _accumulator = TimeSpan.FromTicks(_accumulator.Ticks % _frameRate.Ticks);
            }

            return new MultipleSpriteSelectorState(textures[_currentFrame], textures[_currentFrame].Bounds, _currentFrame);
        }
    }
}
