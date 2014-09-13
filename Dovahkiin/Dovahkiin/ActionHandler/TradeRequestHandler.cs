using System;
using Dovahkiin.Broker;
using Dovahkiin.Model.Action;
using Dovahkiin.Model.Core;

namespace Dovahkiin.ActionHandler
{
    public class TradeRequestHandler : IActionHandler
    {
        public event EventHandler<TradeEventArgs> TradeRequest;
        public event EventHandler<TradeEventArgs> RequestReponse;

        protected virtual void OnRequestReponse(TradeEventArgs e)
        {
            var handler = RequestReponse;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnTradeRequest(TradeEventArgs e)
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
            var accepted = client != null;
            OnRequestReponse(new TradeEventArgs(client) {Accepted = accepted});

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
            var args = new TradeEventArgs(client);
            OnTradeRequest(args);
            return args.Accepted;
        }
    }

    public class TradeEventArgs : EventArgs
    {
        public TradeEventArgs(BrokerClient client)
        {
            Client = client;
        }

        public bool Accepted { get; set; }
        public BrokerClient Client { get; private set; }
    }
}