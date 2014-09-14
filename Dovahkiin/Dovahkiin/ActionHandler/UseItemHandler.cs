using System.Linq;
using Dovahkiin.Model.Action;
using Dovahkiin.Model.Core;

namespace Dovahkiin.ActionHandler
{
    public class UseItemHandler : IActionHandler
    {
        public bool Handle(Actor source, IAction action)
        {
            var useAction = action as UseItem;
            if (useAction == null)
                return false;

            var carrier = source as ICarrier;
            if (carrier == null)
                return false;

            if (!carrier.CarryingItems.Contains(useAction.UsableItem))
                return false;

            useAction.UsableItem.Apply(useAction.Target);
            if (useAction.EndCallback!= null)
                useAction.EndCallback.Invoke(this);

            return true;
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            
        }
    }
}