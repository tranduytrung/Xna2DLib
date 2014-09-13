using System;
using Dovahkiin.ActionHandler;

namespace Dovahkiin.Model.Core
{
    public interface IAction
    {
        string Title { get; set; }
        Action<IActionHandler> EndCallback { get; }
    }
}
