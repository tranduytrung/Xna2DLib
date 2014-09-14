using System;
using Dovahkiin.Broker;
using Dovahkiin.Extension;
using Dovahkiin.Model.Action;
using Dovahkiin.Model.Core;

namespace Dovahkiin.ActionHandler
{
    public class TradeRequestHandler : IActionHandler
    {
        private Actor _source;
        private Actor _target;

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

            var sObj = source as ICanvasObject;
            var tObj = tradeAction.Target as ICanvasObject;
            if (sObj == null || tObj == null)
                return false;

            _source = source;
            _target = tradeAction.Target;

            if (sObj.Distance(tObj) > 50)
            {
                source.DoAction(new Move() { X = tObj.X, Y = tObj.Y, EndCallback = DoTrade });
                return true;
            }

            DoTrade(null);

            if (tradeAction.EndCallback != null)
                tradeAction.EndCallback.Invoke(this);
            return true;
        }

        private void DoTrade(IActionHandler obj)
        {
            var client = TradeBroker.CreateBroker(_source, _target);
            var accepted = client != null;
            OnRequestReponse(new TradeEventArgs(client, (ICarrier)_target) { Accepted = accepted });
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }

        public bool GetReponse(BrokerClient client, ICarrier target)
        {
            var args = new TradeEventArgs(client, target);
            OnTradeRequest(args);
            return args.Accepted;
        }

        public void Dispose()
        {
            
        }
    }

    public class TradeEventArgs : EventArgs
    {
        public TradeEventArgs(BrokerClient client, ICarrier target)
        {
            Client = client;
            Target = target;
        }

        public bool Accepted { get; set; }
        public BrokerClient Client { get; private set; }
        public ICarrier Target { get; set; }
    }
}