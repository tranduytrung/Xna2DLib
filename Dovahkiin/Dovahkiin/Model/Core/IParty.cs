using System;
using System.Collections.Generic;

namespace Dovahkiin.Model.Core
{
    public interface IParty : ICanvasObject
    {
        IEnumerable<ICreature> Members { get; }
        ClanType Clan { get; }
    }

    [Flags]
    public enum ClanType
    {
        None = 0,
        Human = 1,
        Orc = 2,
        Ghost = 4,
        Bandit = 8
    }
}