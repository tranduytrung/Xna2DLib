namespace Dovahkiin.Model.Core
{
    public class Creature : ICreature
    {
        public int ResouceId { get; internal set; }
        public int HitPoint { get; internal set; }
        public int HitGauge { get; internal set; }
        public int MagicPoint { get; internal set; }
        public int MagicGauge { get; internal set; }
    }
}