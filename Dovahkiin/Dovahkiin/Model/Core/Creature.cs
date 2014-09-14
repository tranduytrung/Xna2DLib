namespace Dovahkiin.Model.Core
{
    public abstract class Creature : ICreature
    {
        public abstract int ResouceId { get; }
        public int HitPoint { get; internal set; }
        public abstract int HitGauge { get; }
        public int MagicPoint { get; internal set; }
        public abstract int MagicGauge { get; }
        public int Level { get; private set; }
        public int AccumulatedExperience { get; private set; }
        public abstract int ExperienceGauge { get; }
        public abstract int Power { get; }

        internal void GainExperience(int exp)
        {
            AccumulatedExperience += exp;
            while (AccumulatedExperience >= ExperienceGauge)
            {
                AccumulatedExperience -= ExperienceGauge;
                Level++;
            }
        }

        internal void ModifyHitPoint(int offset)
        {
            HitPoint += offset;
            if (HitPoint < 0)
                HitPoint = 0;
            else if (HitPoint > HitGauge)
                HitPoint = HitGauge;
        }

        internal void ModifyMagicPoint(int offset)
        {
            MagicPoint += offset;
            if (MagicPoint < 0)
                MagicPoint = 0;
            else if (MagicPoint > MagicGauge)
                MagicPoint = MagicGauge;
        }
    }
}