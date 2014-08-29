using System.Collections.Generic;
using tranduytrung.Xna.Core;

namespace tranduytrung.Xna.Engine
{
    internal static class TimerManager
    {
        private static readonly HashSet<Timer> TimerList = new HashSet<Timer>();
        public static void Update()
        {
            foreach (var timer in TimerList)
            {
                timer.Update(GameContext.GameTime.ElapsedGameTime);
            }
        }

        public static void Add(Timer timer)
        {
            TimerList.Add(timer);
        }

        public static void Remove(Timer timer)
        {
            TimerList.Remove(timer);
        }

        public static bool IsRunning(Timer timer)
        {
            return TimerList.Contains(timer);
        }
    }
}
