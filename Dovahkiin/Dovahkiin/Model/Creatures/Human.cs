using Dovahkiin.Constant;
using Dovahkiin.Model.Core;

namespace Dovahkiin.Model.Creatures
{
    public class Human : Creature
    {
        public override int ResouceId
        {
            get { return Textures.Knight; }
        }
    }
}
