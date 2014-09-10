using System.Collections.Generic;

namespace Dovahkiin.Model.Core
{
    public interface IParty : ICanvasObject
    {
        IEnumerable<ICreature> Members { get; }
        ClanType Clan { get; }
    }

    public enum ClanType
    {
        None = 0,
        Human = 1,
        Orc = 2,
        Ghost = 4
    }
}