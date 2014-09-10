using System;
using Dovahkiin.ActionHandler;

namespace Dovahkiin.Model.Core
{
    public interface IAction
    {
        Action<IActionHandler> EndCallback { get; }
    }
}
