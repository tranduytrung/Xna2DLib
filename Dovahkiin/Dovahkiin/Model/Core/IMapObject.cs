using tranduytrung.Xna.Map;

namespace Dovahkiin.Model.Core
{
    public interface IMapObject
    {
        int ResouceId { get; }
        IIsometricDeployable Deployment { get; }
        bool CanOverlap { get; }
    }
}
