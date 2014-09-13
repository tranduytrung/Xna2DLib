using System;
using Dovahkiin.ActionHandler;
using Dovahkiin.Model.Core;

namespace Dovahkiin.Model.Action
{
    public class TradeRequest : IAction
    {
        public Actor Target { get; set; }
        public string Title { get; set; }
        public Action<IActionHandler> EndCallback { get; set; }
    }
}