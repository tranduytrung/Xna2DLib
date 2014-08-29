using System;
using tranduytrung.Xna.Engine;

namespace tranduytrung.Xna.Core
{
    public sealed class Timer
    {
        private TimeSpan _accumulatedTime;
        private int _invokedCount;
        private bool _isGlobal;

        public bool Enable
        {
            get { return IsGlobal? TimerManager.IsRunningOnGlobal(this) : TimerManager.IsRunningOnLocal(this); }
        }

        public TimeSpan Internal { get; set; }
        public int Repeat { get; set; }

        public bool IsGlobal
        {
            get { return _isGlobal; }
            set
            {
                if (Enable)
                    throw new InvalidOperationException("Cannot set this property when the timer is running");

                _isGlobal = value;
            }
        }

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
            if (_invokedCount < Repeat || Repeat == int.MinValue)
            {
                if (Callback != null)
                    Callback.Invoke(this, null);

                ++_invokedCount;
            }
            else
            {
                End();
            }
        }

        public void Start()
        {
            if (IsGlobal)
                TimerManager.AddGlobal(this);
            else
                TimerManager.AddLocal(this);
        }

        public void End()
        {
            if (IsGlobal)
                TimerManager.RemoveGlobal(this);
            else
                TimerManager.RemoveLocal(this);
        }

        public void Reset()
        {
            _accumulatedTime = TimeSpan.Zero;
            _invokedCount = 0;
        }

        public Timer()
        {
            Repeat = int.MinValue;
        }
    }
}