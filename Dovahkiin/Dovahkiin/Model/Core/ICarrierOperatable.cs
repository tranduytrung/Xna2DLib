using System;
using System.Collections.Generic;
using System.Linq;

namespace Dovahkiin.Model.Core
{
    public interface ICarrierOperatable : ICarrier
    {
        CarrierOperator GetOperator();
    }

    public sealed class CarrierOperator
    {
        public CarrierOperator(ICollection<ICarriable> collection)
        {
            Collection = collection;
        }

        public void Add(ICarriable protoItem)
        {
            var itemType = protoItem.GetType();
            var usableTimes = protoItem is Usable ? ((Usable) protoItem).UsableTimes : -1;

            var item = Collection.FirstOrDefault(i => i.GetType() == itemType);

            if (item == null)
            {
                item = (ICarriable)Activator.CreateInstance(itemType);
                Collection.Add(item);
            }

            if (usableTimes != -1)
            {
                ((Usable)item).UsableTimes = usableTimes;
            }
        }

        public ICarriable Get(ICarriable protoItem)
        {
            var itemType = protoItem.GetType();
            var usableTimes = protoItem is Usable ? ((Usable)protoItem).UsableTimes : -1;

            var item = Collection.FirstOrDefault(i => i.GetType() == itemType);
            if (item == null)
                return null;

            if (usableTimes == -1)
            {
                Collection.Remove(item);
                return item;
            }

            var usable = (Usable) item;

            if (usable.UsableTimes <= usableTimes)
            {
                Collection.Remove(usable);
                return usable;
            }

            usable.UsableTimes -= usableTimes;
            var newItem = (Usable)Activator.CreateInstance(itemType);
            newItem.UsableTimes = usableTimes;

            return newItem;
        }

        public ICollection<ICarriable> Collection { get; private set; }
    }

}