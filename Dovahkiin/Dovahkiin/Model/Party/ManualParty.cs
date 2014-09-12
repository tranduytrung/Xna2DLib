using System.Collections.Generic;
using System.Linq;
using Dovahkiin.Constant;
using Dovahkiin.Model.Core;
using Dovahkiin.Model.Item;

namespace Dovahkiin.Model.Party
{
    public class ManualParty : Party
    {
        public override IEnumerable<IAction> GetSuggestionActions(Actor target)
        {
            return Enumerable.Empty<IAction>();
        }

        public override int ResouceId
        {
            get { return Textures.Knight; }
        }

        public ManualParty()
        {
            var bag = GetOperator();
            bag.Add(new SmallBloodPotion() {UsableTimes = 3});
            bag.Add(new Coin() {UsableTimes = 1000});
        }
    }
}