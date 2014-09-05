using Dovahkiin.Model.Core;

namespace Dovahkiin.ActionHandler
{
    public interface IActionHandler
    {
        bool Handle(IAction action);
    }
}
