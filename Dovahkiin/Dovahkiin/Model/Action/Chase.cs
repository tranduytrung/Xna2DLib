using System;
using Dovahkiin.ActionHandler;
using Dovahkiin.Model.Core;

namespace Dovahkiin.Model.Action
{
    public class Chase : IAction
    {
        public ICanvasObject Target { get; set; }
        public Action<IActionHandler> EndCallback { get; set; }
        public double ChaseRange { get; set; }
    }
}