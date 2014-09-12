using System;
using Dovahkiin.Broker;
using Dovahkiin.Model.Action;
using Dovahkiin.Model.Core;

namespace Dovahkiin.ActionHandler
{
    public class TradeRequestHandler : IActionHandler
    {
        public event EventHandler<TradeRequestEventArgs> TradeRequest;

        protected virtual void OnTradeRequest(TradeRequestEventArgs e)
        {
            var handler = TradeRequest;
            if (handler != null) handler(this, e);
        }

        public bool Handle(Actor source, IAction action)
        {
            var tradeAction = action as TradeRequest;
            if (tradeAction == null)
                return false;

            var client = TradeBroker.CreateBroker(source, tradeAction.Target);
            tradeAction.RequestReponse.Invoke(client);

            if (tradeAction.EndCallback != null)
                tradeAction.EndCallback.Invoke(this);
            return true;
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }

        public bool GetReponse(BrokerClient client)
        {
            var args = new TradeRequestEventArgs(client);
            OnTradeRequest(args);
            return args.Accepted;
        }
    }

    public class TradeRequestEventArgs : EventArgs
    {
        public TradeRequestEventArgs(BrokerClient client)
        {
            Client = client;
        }

        public bool Accepted { get; set; }
        public BrokerClient Client { get; private set; }
    }
}