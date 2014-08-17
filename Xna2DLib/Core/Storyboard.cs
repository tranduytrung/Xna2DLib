using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace tranduytrung.Xna.Core
{
    public class Storyboard
    {
        private readonly Collection<IAnimation> _animations = new Collection<IAnimation>();
        private TimeSpan _beginTime;
        private TimeSpan _accumulatedTime;
        private Func<TimeSpan, bool> _phasedAction;

        private bool AccumulationPhase(TimeSpan elapsedTime)
        {
            _accumulatedTime += elapsedTime;
            if (_accumulatedTime >= BeginTime)
            {
                var remainder = _accumulatedTime - BeginTime;
                _phasedAction = AnimationPhase;
                return AnimationPhase(remainder);
            }

            return true;
        }

        private bool AnimationPhase(TimeSpan elapsedTime)
        {
            return (_animations.All((obj) => obj.Update(elapsedTime))) ;
        }

        public ICollection<IAnimation> Animations
        {
            get { return _animations; }
        }

        public TimeSpan BeginTime
        {
            get { return _beginTime; }
            set { _beginTime = value; }
        }

        public Storyboard()
        {
            Reset();
        }

        /// <summary>
        /// Call this method to update the storyboard
        /// </summary>
        /// <param name="elapsedTime">elapsed time since last update</param>
        /// <returns>return true if the storyboard are updated. Otherwise, return false since the storyboard is completed</returns>
        public virtual bool Update(TimeSpan elapsedTime)
        {
            return _phasedAction.Invoke(elapsedTime);
        }

        /// <summary>
        /// Force all animation to revese
        /// </summary>
        public void Reverse()
        {
            foreach (var animation in _animations)
            {
                animation.Reverse();
            }
        }

        /// <summary>
        /// Reset storyboard to begining
        /// </summary>
        public void Reset()
        {
            _accumulatedTime = TimeSpan.Zero;
            _phasedAction = AccumulationPhase;
            foreach (var animation in _animations)
            {
                animation.Reset();
            }
        }
    }
}
