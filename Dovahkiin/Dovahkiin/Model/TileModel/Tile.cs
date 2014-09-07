using Dovahkiin.Model.Core;
using System;
using tranduytrung.Xna.Map;

namespace Dovahkiin.Model.TileModel
{
    public class Tile : IMapObject
    {
        public string Name { get; private set; }
        public bool CanOverlap { get; private set; }


        public Tile(string name, bool overlap)
        {
            Name = name;
            CanOverlap = overlap;
        }

        public IIsometricDeployable Deployment
        {
            get { throw new NotImplementedException(); }
        }
    }
}
