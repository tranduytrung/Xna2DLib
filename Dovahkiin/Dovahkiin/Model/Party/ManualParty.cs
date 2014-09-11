using System.Collections.Generic;
using System.Collections.ObjectModel;
using Dovahkiin.Model.Core;

namespace Dovahkiin.Model.Party
{
    public class ManualParty : Actor, IParty, IMovable, ICarrier
    {
        public ManualParty()
        {
            CarryingItems = new Collection<ICarriable>();
        }

        public IEnumerable<ICreature> Members { get; internal set; }
        public ClanType Clan { get; internal set; }
        public int ResouceId { get; internal set; }
        public int X { get; internal set; }
        public int Y { get; internal set; }
        public int MovingSpeed { get; internal set; }
        public int MaximumCarryCount { get; internal set; }
        public ICollection<ICarriable> CarryingItems { get; private set; }
    }
}