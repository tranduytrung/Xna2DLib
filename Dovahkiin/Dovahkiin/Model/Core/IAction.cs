using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dovahkiin.Model.Core
{
    public interface IAction
    {
        Actor Source { get; }
        Actor Target { get; }
    }
}
