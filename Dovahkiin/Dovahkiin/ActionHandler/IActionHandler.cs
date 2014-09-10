using Dovahkiin.Model.Core;

namespace Dovahkiin.ActionHandler
{
    public interface IActionHandler
    {
        bool Handle(Actor source, IAction action);
        void Stop();
    }
}
