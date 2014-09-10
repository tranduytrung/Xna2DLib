using System.Collections.Generic;
using Dovahkiin.Model.Core;

namespace Dovahkiin.Model.Party
{
    public class ManualParty : Actor, IParty, IMovable
    {
        public IEnumerable<ICreature> Members { get; internal set; }
        public ClanType Clan { get; internal set; }
        public int ResouceId { get; internal set; }
        public int X { get; internal set; }
        public int Y { get; internal set; }
        public int MovingSpeed { get; internal set; }
    }
}