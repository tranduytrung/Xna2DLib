namespace Dovahkiin.Model.Core
{
    public interface IMapObject
    {
        int X { get; }
        int Y { get; }
        bool CanOverlap { get; }
    }
}
