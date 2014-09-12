namespace Dovahkiin.Model.Core
{
    public abstract class Creature : ICreature
    {
        public abstract int ResouceId { get; }
        public int HitPoint { get; internal set; }
        public int HitGauge { get; internal set; }
        public int MagicPoint { get; internal set; }
        public int MagicGauge { get; internal set; }
    }
}