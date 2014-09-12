using Dovahkiin.Constant;

namespace Dovahkiin.Model.Item
{
    public class SmallBloodPotion : BloodPotion
    {
        public override int Volume
        {
            get { return 10; }
        }

        public override int ResouceId
        {
            get { return Textures.BloodPotion; }
        }

        public override int Value
        {
            get { return UsableTimes*BaseValue; }
        }

        public const int BaseValue = 10;
    }
}