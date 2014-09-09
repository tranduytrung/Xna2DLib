using Dovahkiin.Model.Core;

namespace Dovahkiin.Model.Action
{
    public class MoveAction : IAction
    {
        public Actor Target { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
