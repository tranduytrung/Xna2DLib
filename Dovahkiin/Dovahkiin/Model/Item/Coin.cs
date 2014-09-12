using Dovahkiin.Constant;
using Dovahkiin.Model.Core;

namespace Dovahkiin.Model.Item
{
    public class Coin : Usable
    {
        public override int ResouceId
        {
            get { return Textures.BloodPotion; }
        }

        public override string Name
        {
            get { return "Coin"; }
        }

        public override string Description
        {
            get
            {
                return
                    "Money is a strange business. People who haven't got it aim it strongly. People who have are full of troubles -(Ayrton Senna).\nCoin is useless";
            }
        }

        public override int Value
        {
            get { return UsableTimes; }
        }

        protected override bool ApplyEffect(ICreature target)
        {
            return false;
        }
    }
}