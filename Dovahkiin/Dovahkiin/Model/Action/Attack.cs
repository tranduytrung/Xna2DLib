using System;
using Dovahkiin.ActionHandler;
using Dovahkiin.Model.Core;

namespace Dovahkiin.Model.Action
{
    public class Attack : IAction
    {
        public string Title { get; set; }
        public Action<IActionHandler> EndCallback { get; set; }
        public Actor Target { get; set; }
    }
}