using Dovahkiin.Model.Core;

namespace Dovahkiin.Model.Creature
{
    public class Human : Actor, IMovable
    {
        public int ResouceId { get; internal set; }
        public int X { get; internal set; }
        public int Y { get; internal set; }
        public int MovingSpeed { get; internal set; }
    }
}
