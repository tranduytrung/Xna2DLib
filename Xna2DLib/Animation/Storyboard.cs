using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using tranduytrung.Xna.Core;

namespace tranduytrung.Xna.Animation
{
    public class Storyboard
    {
        private readonly Collection<IAnimation> _animations = new Collection<IAnimation>();
        public ICollection<IAnimation> Animations
        {
            get { return _animations; }
        }

        /// <summary>
        /// Call this method to update the storyboard
        /// </summary>
        /// <param name="elapsedTime">elapsed time since last update</param>
        /// <returns>return true if the storyboard are updated. Otherwise, return false since the storyboard is completed</returns>
        public virtual bool Update(TimeSpan elapsedTime)
        {
            return (_animations.All((obj) => obj.Update(elapsedTime))) ;
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
            foreach (var animation in _animations)
            {
                animation.Reset();
            }
        }
    }
}
