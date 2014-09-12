using System.Collections.Generic;
using System.Linq;
using Dovahkiin.Constant;
using Dovahkiin.Model.Core;

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
    }
}