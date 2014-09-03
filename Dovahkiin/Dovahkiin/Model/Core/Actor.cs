using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dovahkiin.Model.Core
{
    public abstract class Actor
    {
        public bool ProcessAction(IAction action)
        {
            return false;
        }

        public abstract IEnumerable<IAction> CanHandleActionCollection { get; }
    }
}
