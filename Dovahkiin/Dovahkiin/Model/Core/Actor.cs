using System.Collections.Generic;

namespace Dovahkiin.Model.Core
{
    public abstract class Actor
    {
        public bool DoAction(IAction action)
        {
            return false;
        }

        public abstract IEnumerable<IAction> CanHandleActionCollection { get; }
    }
}
