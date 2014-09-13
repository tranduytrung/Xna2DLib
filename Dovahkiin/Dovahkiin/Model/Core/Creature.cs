namespace Dovahkiin.Model.Core
{
    public abstract class Creature : ICreature
    {
        public abstract int ResouceId { get; }
        public int HitPoint { get; internal set; }
        public int HitGauge { get; internal set; }
        public int MagicPoint { get; internal set; }
        public int MagicGauge { get; internal set; }
        public int Level { get; internal set; }
        public int AccumulatedExperience { get; internal set; }
        public int ExperienceGauge { get; internal set; }
    }
}