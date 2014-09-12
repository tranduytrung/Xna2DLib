using System.Collections.Generic;

namespace Dovahkiin.Model.Core
{
    public interface ICarrier
    {
        int MaximumCarryCount { get; }
        IEnumerable<ICarriable> CarryingItems { get; }
    }
}
