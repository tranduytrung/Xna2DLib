using System.Collections.Generic;
using Microsoft.Xna.Framework;
using tranduytrung.Xna.Core;

namespace tranduytrung.Xna.Engine
{
    internal static class TimerManager
    {
        public static void AddGlobal(Timer timer)
        {
            GameContext.GameInstance.GlobalTimerList.Add(timer);
        }

        public static void AddLocal(Timer timer)
        {
            GameContext.GameInstance.ActiveScreen.LocalTimerList.Add(timer);
        }

        public static void RemoveGlobal(Timer timer)
        {
            GameContext.GameInstance.GlobalTimerList.Remove(timer);
        }

        public static void RemoveLocal(Timer timer)
        {
            GameContext.GameInstance.ActiveScreen.LocalTimerList.Remove(timer);
        }

        public static bool IsRunningOnGlobal(Timer timer)
        {
            return GameContext.GameInstance.GlobalTimerList.Contains(timer);
        }

        public static bool IsRunningOnLocal(Timer timer)
        {
            return GameContext.GameInstance.ActiveScreen.LocalTimerList.Contains(timer);
        }
    }
}
