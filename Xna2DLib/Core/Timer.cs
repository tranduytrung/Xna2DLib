using System;
using tranduytrung.Xna.Engine;

namespace tranduytrung.Xna.Core
{
    public sealed class Timer
    {
        private TimeSpan _accumulatedTime;
        private int _invokedCount;

        public bool Enable
        {
            get { return TimerManager.IsRunning(this); }
        }

        public TimeSpan Internal { get; set; }
        public int Repeat { get; set; }

        public TimeSpan AccumulatedTime
        {
            get { return _accumulatedTime; }
        }

        public int InvokedCount
        {
            get { return _invokedCount; }
        }

        public event EventHandler Callback;

        internal void Update(TimeSpan elapsedTime)
        {
            _accumulatedTime += elapsedTime;
            if (_accumulatedTime < Internal) return;

            _accumulatedTime -= Internal;
            if (_invokedCount < Repeat)
            {
                if (Callback != null)
                    Callback.Invoke(this, null);

                ++_invokedCount;
            }
            else
            {
                TimerManager.Remove(this);
            }
        }

        public void Start()
        {
            TimerManager.Add(this);
        }

        public void End()
        {
            TimerManager.Remove(this);
        }

        public void Reset()
        {
            _accumulatedTime = TimeSpan.Zero;
            _invokedCount = 0;
        }
    }
}