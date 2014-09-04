using Dovahkiin.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dovahkiin.Model.TileModel
{
    public class Tile : IMapObject
    {
        public string Name { get; private set; }
        public bool CanOverlap { get; private set; }

        public int X
        {
            get { throw new NotImplementedException(); }
        }

        public int Y
        {
            get { throw new NotImplementedException(); }
        }

        public Tile(string name, bool overlap)
        {
            Name = name;
            CanOverlap = overlap;
        }
    }
}
