using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Dovahkiin.Model.Core;

namespace Dovahkiin.Model.Party
{
    public abstract class Party : Actor, IParty, IMovable, ICarrier, ICarrierOperatable
    {
        internal class CarriableCollection : KeyedCollection<Type, ICarriable>
        {
            protected override Type GetKeyForItem(ICarriable item)
            {
                return item.GetType();
            }
        }

        private readonly CarriableCollection _carryingItems = new CarriableCollection();
        public abstract int ResouceId { get; }
        public IEnumerable<ICreature> Members { get; internal set; }
        public ClanType Clan { get; internal set; }
        public int X { get; internal set; }
        public int Y { get; internal set; }
        public int MovingSpeed { get; internal set; }
        public int MaximumCarryCount { get; internal set; }

        public IEnumerable<ICarriable> CarryingItems
        {
            get { return _carryingItems; }
        }

        public CarrierOperator GetOperator()
        {
            return new CarrierOperator(_carryingItems);
        }
    }
}