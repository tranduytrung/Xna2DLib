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

        private void OnReceivedRequest(object sender, TradeRequestEventArgs e)
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
            var amountOfCoin = Math.Min(_client.DemandItems.Sum(item => item.Value / 4), GetAvailableCoin());
            if (
                _client.OfferItems.FirstOrDefault(
                    item => item.GetType() == typeof(Coin) && ((Coin)item).Value <= amountOfCoin) != null)
            {
                _client.Accept();
            }
            else
            {
                _client.Submit(new List<ICarriable>{ new Coin() {UsableTimes = amountOfCoin} }, _client.DemandItems);
            }
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
            _client = null;
        }

        public override IEnumerable<IAction> GetSuggestionActions(Actor target)
        {
            yield return new TradeRequest() {Target = this};
        }

        public override int ResouceId
        {
            get { return Textures.Knight; }
        }
    }
}