using tranduytrung.Xna.Map;

namespace Dovahkiin.Model.Core
{
    public interface IMapObject
    {
        IIsometricDeployable Deployment { get; }
        bool CanOverlap { get; }
    }
}
