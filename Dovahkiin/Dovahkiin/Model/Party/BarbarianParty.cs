using System.Collections.Generic;
using Dovahkiin.ActionHandler;
using Dovahkiin.Constant;
using Dovahkiin.Model.Action;
using Dovahkiin.Model.Core;

namespace Dovahkiin.Model.Party
{
    class BarbarianParty : Party
    {
        public override IEnumerable<IAction> GetSuggestionActions(Actor target)
        {
            yield return new Attack() {Target = this, Title = "Attack!!"};
        }

        public override int ResouceId
        {
            get { return Textures.Knight; }
        }

        public BarbarianParty()
        {
            MovingSpeed = 100;
            Clan = ClanType.Human | ClanType.Bandit;
            AddActionHandler(new AgressiveLookHandler() { SightRange = 300 });
            AddActionHandler(new ChaseHandler());
            AddActionHandler(new MoveHandler());
            var attackHandler = new AttackHandler();
            AddActionHandler(attackHandler);
            DoAction(new AgressiveLook() { AlliesClan = ClanType.Bandit });
        }
    }
}