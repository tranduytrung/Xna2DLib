using System;
using Dovahkiin.Extension;
using Dovahkiin.Model.Action;
using Dovahkiin.Model.Core;
using tranduytrung.Xna.Core;

namespace Dovahkiin.ActionHandler
{
    public class ChaseHandler : IActionHandler
    {
        private readonly Timer _refocusTimer;
        private Chase _currentAction;
        private Actor _currentSource;
        public TimeSpan RefocusInterval { get; set; }
        public bool Handle(Actor source, IAction action)
        {
            var chase = action as Chase;
            if (chase == null)
                return false;

            if (!(source is IMovable))
                return false;

            Stop();
            _currentAction = chase;
            _currentSource = source;

            _refocusTimer.Internal = RefocusInterval;
            _refocusTimer.Start();

            return true;
        }

        public void Stop()
        {
            _refocusTimer.End();
            Finish();
        }

        private void Finish()
        {
            if (_currentAction != null && _currentAction.EndCallback != null)
            {
                var callback = _currentAction.EndCallback;
                _currentAction = null;
                _currentSource = null;
                callback.Invoke(this);
            }
        }

        public ChaseHandler()
        {
            RefocusInterval = TimeSpan.FromSeconds(1.5);
            _refocusTimer = new Timer();
            _refocusTimer.Callback += OnFocus;
        }

        private void OnFocus(object sender, EventArgs e)
        {
            var chaseRange = _currentAction.ChaseRange;
            var distance = _currentAction.Target.DistanceSquare((ICanvasObject) _currentSource);
            if (distance < 50)
                Stop();
            else if (distance <= chaseRange*chaseRange)
                _currentSource.DoAction(new Move() {X = _currentAction.Target.X, Y = _currentAction.Target.Y});
            else
                Stop();
        }

        public void Dispose()
        {
            _refocusTimer.End();
        }
    }
}