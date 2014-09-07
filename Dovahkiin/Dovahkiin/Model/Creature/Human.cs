using Dovahkiin.Model.Core;
using System;
using System.Collections.Generic;
using tranduytrung.Xna.Map;

namespace Dovahkiin.Model.Creature
{
    public class Human : Actor, IMapObject
    {
        public IIsometricDeployable Deployment { get; private set; }
        public bool CanOverlap { get { return false; } }

        public override IEnumerable<IAction> CanHandleActionCollection
        {
            get { throw new NotImplementedException(); }
        }
    }
}
