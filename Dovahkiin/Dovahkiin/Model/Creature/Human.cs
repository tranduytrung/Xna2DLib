using Dovahkiin.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dovahkiin.Model.Creature
{
    public class Human : Actor, IMapObject
    {
        public int X { get; internal set; }

        public int Y { get; internal set;}
        public bool CanOverlap { get { return false; } }

        public override IEnumerable<IAction> CanHandleActionCollection
        {
            get { throw new NotImplementedException(); }
        }
    }
}
