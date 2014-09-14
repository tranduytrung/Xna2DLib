using System;
using System.Linq;
using Dovahkiin.Extension;
using Dovahkiin.Model.Action;
using Dovahkiin.Model.Core;
using Dovahkiin.Repository;
using tranduytrung.Xna.Core;

namespace Dovahkiin.ActionHandler
{
    public class AgressiveLookHandler : IActionHandler
    {
        private readonly Timer _lookTimer;
        private AgressiveLook _currentAction;
        private Actor _currentSource;

        public TimeSpan LookInterval { get; set; }
        public double SightRange { get; set; }

        public bool Handle(Actor source, IAction action)
        {
            var lookAction = action as AgressiveLook;

            if (lookAction == null)
                return false;

            if (!(source is IParty) || !(source is IMovable))
                return false;

            Stop();
            _currentAction = lookAction;
            _currentSource = source;

            _lookTimer.Internal = LookInterval;
            _lookTimer.Start();

            return true;
        }

        public void Stop()
        {
            _lookTimer.End();
            Finish();
        }

        private void Finish()
        {
            if (_currentAction != null && _currentAction.EndCallback != null)
                _currentAction.EndCallback.Invoke(this);

            _currentAction = null;
            _currentSource = null;
        }

        public AgressiveLookHandler()
        {
            LookInterval = TimeSpan.FromSeconds(3);
            _lookTimer = new Timer();
            _lookTimer.Callback += OnLook;
        }

        private void OnLook(object sender, EventArgs e)
        {
            var map = DataContext.Current.Map;
            var sightSquarre = SightRange*SightRange;
            var enemy = map.CanvasObjects.FirstOrDefault(target =>
            {
                if (target == _currentSource)
                    return false;
                var party = target as IParty;
                return party != null && !party.Clan.HasFlag(_currentAction.AlliesClan) && party.DistanceSquare((ICanvasObject)_currentSource) <= sightSquarre;
            });

            if (enemy == null) return;

            // attack him
            if (enemy.Distance((ICanvasObject) _currentSource) < 50)
            {
                _currentSource.DoAction(new Attack() { Target = (Actor)enemy });
                return;
            }

            // chase him
            _lookTimer.End();
            _currentSource.DoAction(new Chase() {Target = enemy, ChaseRange = SightRange*2, EndCallback = Rework});
        }

        private void Rework(IActionHandler obj)
        {
            if (_currentSource == null || _currentAction == null)
                return;

           OnLook(null, null);
           _lookTimer.Start();
        }

        public void Dispose()
        {
            _lookTimer.End();
        }
    }
}