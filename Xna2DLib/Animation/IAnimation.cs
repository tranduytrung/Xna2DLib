using System;

namespace tranduytrung.Xna.Core
{
    public interface IAnimation
    {
        /// <summary>
        /// Update the animation, so the animation can process correctly
        /// </summary>
        /// <param name="elapsedTime">elapsed time since last update</param>
        /// <returns>true if successfully updated. Otherwise, false.</returns>
        bool Update(TimeSpan elapsedTime);
        /// <summary>
        /// Force to reverse
        /// </summary>
        void Reverse();
        /// <summary>
        /// Reset the animation to beginning state
        /// </summary>
        void Reset();
    }
}
