using System.Collections.Generic;
using tranduytrung.Xna.Animation;

namespace tranduytrung.Xna.Engine
{
    public static class AnimationManager
    {
        private static readonly HashSet<Storyboard> StoryboardCollection = new HashSet<Storyboard>();

        /// <summary>
        /// Begin an animation
        /// </summary>
        /// <param name="storyboard">animation script</param>
        public static void BeginAnimation(Storyboard storyboard)
        {
            StoryboardCollection.Add(storyboard);
        }

        /// <summary>
        /// Stop the animation
        /// </summary>
        /// <param name="storyboard">the instance that animating</param>
        public static void EndAnimation(Storyboard storyboard)
        {
            StoryboardCollection.Remove(storyboard);
        }

        /// <summary>
        /// Check if the script is working
        /// </summary>
        /// <param name="storyboard">the story instance</param>
        /// <returns>true if the storyboard is executing. Otherwise, false</returns>
        public static bool IsAnimating(Storyboard storyboard)
        {
            return StoryboardCollection.Contains(storyboard);
        }

        /// <summary>
        /// Update all animations
        /// </summary>
        internal static void Update()
        {
            // Animation
            StoryboardCollection.RemoveWhere(obj => !obj.Update(GameContext.GameTime.ElapsedGameTime));
        }
    }
}
