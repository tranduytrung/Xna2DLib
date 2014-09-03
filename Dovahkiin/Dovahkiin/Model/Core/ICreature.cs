namespace Dovahkiin.Model.Core
{
    public interface ICreature
    {
        int HitPoint { get; }
        int HitGauge { get; }
        int MagicPoint { get; }
        int MagicGauge { get; }
    }
}
