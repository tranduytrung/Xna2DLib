using System;
using Dovahkiin.ActionHandler;
using Dovahkiin.Model.Core;

namespace Dovahkiin.Model.Action
{
    public class Move : IAction
    {
        public Action<IActionHandler> EndCallback { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
