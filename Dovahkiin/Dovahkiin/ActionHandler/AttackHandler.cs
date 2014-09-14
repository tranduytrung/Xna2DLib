using System;
using Dovahkiin.Extension;
using Dovahkiin.Model.Action;
using Dovahkiin.Model.Core;

namespace Dovahkiin.ActionHandler
{
    public class AttackHandler : IActionHandler
    {
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

            var sParty = source as IParty;
            var tParty = attackAction.Target as IParty;
            if (sParty == null || tParty == null)
                return false;

            var handler = (AttackHandler)attackAction.Target.GetActionHandler(typeof(AttackHandler));

            if (handler == null)
                return false;

            if (sParty.Distance(tParty) > 50)
                return false;

            handler.AttackFrom(sParty);
            OnAttacked(new AttackEventArgs(tParty));

            return true;
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