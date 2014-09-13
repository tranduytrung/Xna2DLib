using System;
using System.Collections.Generic;
using System.Linq;
using Dovahkiin.ActionHandler;
using Dovahkiin.Model.Core;

namespace Dovahkiin.Broker
{
    public sealed class TradeBroker
    {
        private BrokerClient _requestClient;
        private BrokerClient _responseClient;
        private List<ICarriable> _requestorItems;
        private List<ICarriable> _responsorItems;
        private bool _requestorAccepted;
        private bool _reponsorAccepted;

        public ICarrierOperatable Requestor { get; private set; }
        public ICarrierOperatable Responsor { get; private set; }
        public event EventHandler DealChanged;
        public event EventHandler DealAccepted;

        private void OnDealAccepted()
        {
            EventHandler handler = DealAccepted;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private void OnDealChanged()
        {
            EventHandler handler = DealChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public IEnumerable<ICarriable> RequestorItems
        {
            get { return _requestorItems; }
        }

        public IEnumerable<ICarriable> ResponsorItems
        {
            get { return _responsorItems; }
        }

        private TradeBroker()
        {
            _requestorItems = new List<ICarriable>();
            _responsorItems = new List<ICarriable>();
        }

        public static BrokerClient CreateBroker(Actor requestor, Actor responsor)
        {
            if (!(requestor is ICarrierOperatable && responsor is ICarrierOperatable))
                return null;

            var broker = new TradeBroker { Responsor = (ICarrierOperatable)responsor, Requestor = (ICarrierOperatable)requestor };
            broker._requestClient = new BrokerClient(broker);
            broker._responseClient = new BrokerClient(broker);

            var actionHandler = (TradeRequestHandler)responsor.ActionHandlers.FirstOrDefault(handler => handler.GetType() == typeof(TradeRequestHandler));
            if (actionHandler == null)
                return null;

            return actionHandler.GetReponse(broker._responseClient, requestor) ? broker._requestClient : null;
        }

        public void Submit(BrokerClient brokerClient, IEnumerable<ICarriable> offerItems, IEnumerable<ICarriable> demandItems)
        {
            if (_reponsorAccepted && _requestorAccepted)
                throw new ObjectDisposedException("the broker is dispose or deal is completed");

            if (brokerClient == _requestClient)
            {
                _requestorItems = new List<ICarriable>(offerItems);
                _responsorItems = new List<ICarriable>(demandItems);
                _requestorAccepted = true;
                _reponsorAccepted = false;
            }
            else
            {
                _requestorItems = new List<ICarriable>(demandItems);
                _responsorItems = new List<ICarriable>(offerItems);
                _requestorAccepted = false;
                _reponsorAccepted = true;
            }

            OnDealChanged();
        }

        public void Accept(BrokerClient brokerClient)
        {
            if (brokerClient == _requestClient)
                _requestorAccepted = true;
            else
                _reponsorAccepted = true;

            if (_reponsorAccepted && _requestorAccepted)
            {
                if (!IsValid())
                    throw new InvalidOperationException("invalid trade deal: some item do not belong to owner");

                ExchangeItems();
                OnDealAccepted();
            }
        }

        private bool IsValid()
        {
            var requestOperator = Requestor.GetOperator();
            if (RequestorItems.Select(item => requestOperator.Collection.Any(x =>
            {
                if (x.GetType() != item.GetType())
                    return false;

                if (!(item is Usable))
                    return true;

                return ((Usable) x).UsableTimes >= ((Usable) item).UsableTimes;
            })).Any(valid => !valid))
            {
                return false;
            }

            var responseOperator = Responsor.GetOperator();
            return ResponsorItems.Select(item => responseOperator.Collection.Any(x =>
            {
                if (x.GetType() != item.GetType())
                    return false;

                if (!(item is Usable))
                    return true;

                return ((Usable) x).UsableTimes >= ((Usable) item).UsableTimes;
            })).All(valid => valid);
        }

        private void ExchangeItems()
        {
            var reqOpt = Requestor.GetOperator();
            var resOpt = Responsor.GetOperator();
            foreach (var item in RequestorItems)
            {
                resOpt.Add(reqOpt.Get(item));
            }

            foreach (var item in ResponsorItems)
            {
                reqOpt.Add(resOpt.Get(item));
            }
        }

        public IEnumerable<ICarriable> GetDemandItems(BrokerClient brokerClient)
        {
            return brokerClient == _requestClient ? _responsorItems : _requestorItems;
        }

        public IEnumerable<ICarriable> GetOfferItems(BrokerClient brokerClient)
        {
            return brokerClient == _requestClient ? _requestorItems : _responsorItems;
        }
    }

    public sealed class BrokerClient
    {
        private readonly TradeBroker _broker;
        public event EventHandler DealChanged;
        public event EventHandler DealAccepted;

        public IEnumerable<ICarriable> DemandItems
        {
            get { return _broker.GetDemandItems(this); }
        }

        public IEnumerable<ICarriable> OfferItems
        {
            get { return _broker.GetOfferItems(this); }
        }

        public BrokerClient(TradeBroker broker)
        {
            _broker = broker;
            _broker.DealChanged += OnDealChanged;
            _broker.DealAccepted += OnDealAccepted;
        }

        private void OnDealChanged()
        {
            EventHandler handler = DealChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private void OnDealAccepted()
        {
            EventHandler handler = DealAccepted;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private void OnDealChanged(object sender, EventArgs e)
        {
            OnDealChanged();
        }

        private void OnDealAccepted(object sender, EventArgs e)
        {
            OnDealAccepted();
        }

        public void Submit(IEnumerable<ICarriable> offerItems, IEnumerable<ICarriable> demandItems)
        {
            _broker.Submit(this, offerItems, DemandItems);
        }

        public void Accept()
        {
            _broker.Accept(this);
        }
    }
}
