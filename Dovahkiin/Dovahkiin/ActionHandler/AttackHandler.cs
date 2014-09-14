using System;
using Dovahkiin.Extension;
using Dovahkiin.Model.Action;
using Dovahkiin.Model.Core;

namespace Dovahkiin.ActionHandler
{
    public class AttackHandler : IActionHandler
    {
        private AttackHandler _handler;
        private IParty _source;
        private IParty _target;

        public event EventHandler<AttackEventArgs> Attacked;

        protected virtual void OnAttacked(AttackEventArgs e)
        {
            var handler = Attacked;
            if (handler != null) handler(this, e);
        }

        public bool Handle(Actor source, IAction action)
        {
            var attackAction = action as Attack;
            if (attackAction == null)
                return false;

            _source = source as IParty;
            _target = attackAction.Target as IParty;
            if (_source == null || _target == null)
                return false;

            _handler = (AttackHandler)attackAction.Target.GetActionHandler(typeof(AttackHandler));

            if (_handler == null)
                return false;

            if (_source.Distance(_target) > 50)
            {
                source.DoAction(new Move() { X = _target.X, Y = _target.Y, EndCallback = StartAttack });
                return true;
            }

            _handler.AttackFrom(_source);
            OnAttacked(new AttackEventArgs(_target));

            return true;
        }

        private void StartAttack(IActionHandler obj)
        {
            _handler.AttackFrom(_source);
            OnAttacked(new AttackEventArgs(_target));
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }

        public void AttackFrom(IParty target)
        {
            OnAttacked(new AttackEventArgs(target));
        }

        public void Dispose()
        {
        }
    }

    public class AttackEventArgs : EventArgs
    {
        public AttackEventArgs(IParty target)
        {
            Target = target;
        }

        public IParty Target { get; private set; }
    }
}