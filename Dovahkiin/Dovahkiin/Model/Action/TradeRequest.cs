using System;
using Dovahkiin.ActionHandler;
using Dovahkiin.Broker;
using Dovahkiin.Model.Core;

namespace Dovahkiin.Model.Action
{
    public class TradeRequest : IAction
    {
        public Actor Target { get; set; }
        public Action<IActionHandler> EndCallback { get; set; }
        public Action<BrokerClient> RequestReponse { get; set; }
    }
}