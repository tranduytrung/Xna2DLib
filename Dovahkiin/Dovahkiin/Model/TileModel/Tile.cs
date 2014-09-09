using Dovahkiin.Model.Core;
using System;
using tranduytrung.Xna.Map;

namespace Dovahkiin.Model.TileModel
{
    public class Tile : IMapObject
    {
        public int ResouceId { get; private set; }
        public string Name { get; private set; }
        public bool CanOverlap { get; private set; }


        public Tile(int resouceId, string name, bool overlap)
        {
            ResouceId = resouceId;
            Name = name;
            CanOverlap = overlap;
        }

        public IIsometricDeployable Deployment
        {
            get { throw new NotImplementedException(); }
        }
    }
}
