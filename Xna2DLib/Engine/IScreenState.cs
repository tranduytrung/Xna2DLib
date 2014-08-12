using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tranduytrung.Xna.Engine
{
    public interface IScreenState
    {
        bool ExecuteDraw { get; set; }
        bool ExecuteUpdate { get; set; }
        void TransitFrom(IScreenState state);
        void Back();
    }
}
