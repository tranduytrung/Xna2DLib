using System;
using System.Collections.Generic;
using System.Linq;
using Dovahkiin.ActionHandler;
using Dovahkiin.Broker;
using Dovahkiin.Constant;
using Dovahkiin.Model.Action;
using Dovahkiin.Model.Core;
using Dovahkiin.Model.Item;

namespace Dovahkiin.Model.Party
{
    public class TraderParty : Party
    {
        private BrokerClient _client;
        public TraderParty()
        {
            var tradeHandller = new TradeRequestHandler();
            tradeHandller.TradeRequest += OnReceivedRequest;
            AddActionHandler(tradeHandller);
        }

        private void OnReceivedRequest(object sender, TradeEventArgs e)
        {
            if (_client != null)
            {
                e.Accepted = false;
                return;
            }

            e.Accepted = true;
            _client = e.Client;
            _client.DealChanged += OnDealChanged;
            _client.DealAccepted += OnDealAccepted;
        }

        private void OnDealAccepted(object sender, EventArgs e)
        {
            _client = null;
        }

        private int GetAvailableCoin()
        {
            var coin = (Coin)CarryingItems.FirstOrDefault(item => item.GetType() == typeof (Coin));
            if (coin == null)
                return 0;

            return coin.Value;
        }

        private void OnDealChanged(object sender, EventArgs e)
        {
            var payCoin = Math.Min(_client.DemandItems.Sum(item => item.Value / 2), GetAvailableCoin());
            var acceptDemand = !_client.OfferItems.OfType<Coin>().TakeWhile(coin => coin.UsableTimes > payCoin).Any();

            var receiveCoin = _client.OfferItems.Sum(item => item.Value);
            var acceptOffer = !_client.DemandItems.OfType<Coin>().TakeWhile(coin => coin.UsableTimes < receiveCoin).Any();

            if (acceptDemand && acceptOffer)
            {
                _client.Accept();
                return;
            }

            if (!acceptDemand)
            {
                var coin = (Coin)_client.OfferItems.First(item => item.GetType() == typeof (Coin));
                coin.UsableTimes = payCoin;
            }

            var demandList = _client.DemandItems.ToList();
            if (!acceptOffer)
            {
                var coin = (Coin)_client.DemandItems.FirstOrDefault(item => item.GetType() == typeof(Coin));
                if (coin == null)
                {
                    coin = new Coin();
                    demandList.Add(coin);
                }

                coin.UsableTimes = receiveCoin;
            }

            _client.Submit(_client.OfferItems, demandList);
        }

        public override IEnumerable<IAction> GetSuggestionActions(Actor target)
        {
            yield return new TradeRequest() {Target = this, Title = "Cake, juice, sword or .. girl?"};
        }

        public override int ResouceId
        {
            get { return Textures.Knight; }
        }
    }
}