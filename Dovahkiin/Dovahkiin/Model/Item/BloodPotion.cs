using Dovahkiin.Model.Core;

namespace Dovahkiin.Model.Item
{
    public abstract class BloodPotion : Usable
    {
        public override string Name { get { return @"Blood Potion"; } }
        public override string Description { get { return string.Format("Restore {0} HP", Volume); } }
        public abstract int Volume { get; }

        protected override bool ApplyEffect(ICreature target)
        {
            var creature = (Creature) target;
            creature.HitPoint += Volume;
            return true;
        }
    }
}