using Dovahkiin.Model.Core;

namespace Dovahkiin.Model.Creature
{
    public class Human : ICreature
    {
        public int ResouceId { get; internal set; }
        public int HitPoint { get; internal set; }
        public int HitGauge { get; internal set; }
        public int MagicPoint { get; internal set; }
        public int MagicGauge { get; internal set; }
    }
}
