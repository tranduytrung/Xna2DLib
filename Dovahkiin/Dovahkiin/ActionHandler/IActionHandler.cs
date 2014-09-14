using System;
using Dovahkiin.Model.Core;

namespace Dovahkiin.ActionHandler
{
    public interface IActionHandler: IDisposable
    {
        bool Handle(Actor source, IAction action);
        void Stop();
    }
}
