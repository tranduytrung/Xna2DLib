using System;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Helper;

namespace tranduytrung.Xna.Animation
{
    public abstract class Animation<T> : IAnimation, ICloneable
    {
        private TimeSpan _accumulatedTime;
        private int _repeatCount;
        private Func<TimeSpan, bool> _phasingFunction;

        protected SetAccessorDelegate<T> SetAccessor { get; private set; }
        protected GetAccessorDelegate<T> GetAccessor { get; private set; }

        protected Animation(GetAccessorDelegate<T> getAccessor, SetAccessorDelegate<T> setAccessor)
        {
            SetAccessor = setAccessor;
            GetAccessor = getAccessor;
            ResetBase();
        }

        protected Animation(object target, string propertyPath)
        {
            SetAccessorDelegate<T> setAccessor;
            GetAccessorDelegate<T> getAccessor;
            target.ExtractAccessors(propertyPath, out getAccessor, out setAccessor);

            if (setAccessor == null)
                throw new ArgumentException(string.Format("{0} has no set accessor",propertyPath), propertyPath);

            if (getAccessor == null)
                throw new ArgumentException(string.Format("{0} has no get accessor",propertyPath), propertyPath);

            SetAccessor = setAccessor;
            GetAccessor = getAccessor;
            ResetBase();
        }

        public int RepeatTime { get; set; }

        public TimeSpan RepeatDelay { get; set; }

        public bool AutoReverve { get; set; }

        public TimeSpan Duration { get; set; }

        public TimeSpan BeginTime { get; set; }

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

        public void ResetBase()
        {
            _accumulatedTime = TimeSpan.Zero;
            _repeatCount = 0;
            _phasingFunction = WaitingPhase;
        }

        public virtual void Reset()
        {
            ResetBase();
        }

        public virtual void Initialize()
        {
            
        }

        /// <summary>
        /// The function of animation
        /// </summary>
        /// <param name="elapsedProportion">proportion, range from 0 to 1</param>
        /// <returns>value that will return from the function</returns>
        protected abstract T AnimationFunction(double elapsedProportion);

        public abstract object Clone();


        private bool WaitingPhase(TimeSpan elapsedTime)
        {
            _accumulatedTime += elapsedTime;
            if (_accumulatedTime >= BeginTime)
            {
                Initialize();
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
                SetAccessor.Invoke(value);
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
                SetAccessor.Invoke(value);
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
                SetAccessor.Invoke(value);
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
                SetAccessor.Invoke(value);
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
