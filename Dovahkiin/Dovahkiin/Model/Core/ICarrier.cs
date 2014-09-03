using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dovahkiin.Model.Core
{
    public interface ICarrier
    {
        int MaximumCarryCount { get; }
        ICollection<ICarriable> CarryingItems { get; set; }
    }
}
