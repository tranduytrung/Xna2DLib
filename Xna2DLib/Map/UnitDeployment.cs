using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tranduytrung.Xna.Map
{
    public class UnitDeployment : IIsometricDeployable
    {
        private IsometricCoords _position;

        public IsometricCoords Left
        {
            get { return _position; }
        }

        public IsometricCoords Right
        {
            get { return _position; }
        }
        public IsometricCoords Top
        {
            get { return _position; }
        }
        public IsometricCoords Bottom
        {
            get { return _position; }
        }
        public int Level { get; set; }
        public IEnumerable<IsometricCoords> Formation
        {
            get { yield return _position; }
        }
        public void Deploy(IsometricCoords position, double x = 0.5, double y = 0.5)
        {
            _position = position;
        }
    }
}
