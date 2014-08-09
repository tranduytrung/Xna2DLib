using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tranduytrung.Xna.Core
{
    public abstract class Animation : IAnimation, ICloneable
    {
        private TimeSpan _accumulatedTime;
        private int _repeatCount;
        private Func<TimeSpan, bool> _phasingFunction;

        private int _repeatTime;
        private TimeSpan _repeatDelay;
        private bool _autoReverve;
        private TimeSpan _duration;
        private TimeSpan _beginTime;

        public Animation()
        {
            Reset();
        }

        public int RepeatTime
        {
            get { return _repeatTime; }
            set { _repeatTime = value; }
        }

        public TimeSpan RepeatDelay
        {
            get { return _repeatDelay; }
            set { _repeatDelay = value; }
        }

        public bool AutoReverve
        {
            get { return _autoReverve; }
            set { _autoReverve = value; }
        }

        public TimeSpan Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        public TimeSpan BeginTime
        {
            get { return _beginTime; }
            set { _beginTime = value; }
        }

        public bool Update(TimeSpan elapsedTime)
        {
            return _phasingFunction.Invoke(elapsedTime);
        }

        public void Reverse()
        {
            if (_phasingFunction != AnimationPhase)
            {
                _accumulatedTime = Duration;
            }
            _phasingFunction = ReversingPhase;
        }

        public void Reset()
        {
            _accumulatedTime = TimeSpan.Zero;
            _repeatCount = 0;
            _phasingFunction = WaitingPhase;
        }

        public event Action<object> AnimationCallback;

        /// <summary>
        /// The function of animation
        /// </summary>
        /// <param name="elapsedProportion">proportion, range from 0 to 1</param>
        /// <returns>value that will return from the function</returns>
        protected abstract object AnimationFunction(double elapsedProportion);

        public abstract object Clone();

        private bool WaitingPhase(TimeSpan elapsedTime)
        {
            _accumulatedTime += elapsedTime;
            if (_accumulatedTime >= BeginTime)
            {
                var remainder = _accumulatedTime - BeginTime;
                _phasingFunction = AnimationPhase;
                _accumulatedTime = TimeSpan.Zero;
                return AnimationPhase(remainder);
            }

            return true;
        }

        private bool AnimationPhase(TimeSpan elapsedTime)
        {
            _accumulatedTime += elapsedTime;
            var proportion = (double) _accumulatedTime.Ticks/Duration.Ticks;
            if (proportion >= 1)
            {
                var remainder = _accumulatedTime - Duration;
                var value = AnimationFunction(1.0);
                AnimationCallback.Invoke(value);
                if (AutoReverve)
                {
                    // Reverse phase
                    _accumulatedTime = Duration;
                    _phasingFunction = ReversingPhase;
                }
                else
                {
                    // Repeat phase
                    if (_repeatCount >= RepeatTime)
                    {
                        _accumulatedTime = Duration;
                        _phasingFunction = EndingPhase;
                    }
                    else
                    {
                        _accumulatedTime = TimeSpan.Zero;
                        ++_repeatCount;
                        _phasingFunction = RepeatDelayPhase;
                    }
                }
                _phasingFunction.Invoke(remainder);
            }
            else
            {
                var value = AnimationFunction(proportion);
                AnimationCallback.Invoke(value);
            }

            return true;
        }

        private bool ReversingPhase(TimeSpan elapsedTime)
        {
            _accumulatedTime -= elapsedTime;
            var proportion = (double) _accumulatedTime.Ticks/Duration.Ticks;
            if (proportion <= 0)
            {
                var remainder = _accumulatedTime - Duration;
                var value = AnimationFunction(0.0);
                AnimationCallback.Invoke(value);
                _accumulatedTime = Duration;
                if (_repeatCount >= RepeatTime)
                {
                    _accumulatedTime = TimeSpan.Zero;
                    _phasingFunction = EndingPhase;
                }
                else
                {
                    ++_repeatCount;
                    _phasingFunction = RepeatDelayPhase;
                    RepeatDelayPhase(remainder);
                }
            }
            else
            {
                var value = AnimationFunction(proportion);
                AnimationCallback.Invoke(value);
            }

            return true;
        }

        private bool RepeatDelayPhase(TimeSpan elapsedTime)
        {
            _accumulatedTime += elapsedTime;

            if (_accumulatedTime >= RepeatDelay)
            {
                var remainder = _accumulatedTime - RepeatDelay;
                _phasingFunction = AnimationPhase;
                _accumulatedTime = TimeSpan.Zero;
                return AnimationPhase(remainder);
            }

            return true;
        }

        private bool EndingPhase(TimeSpan elapsedTime)
        {
            return false;
        }
    }
}
