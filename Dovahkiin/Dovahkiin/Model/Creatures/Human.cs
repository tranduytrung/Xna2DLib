using System;
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

        public override int HitGauge
        {
            get { return 30 + Level*5; }
        }

        public override int MagicGauge
        {
            get { return 20 + Level*3; }
        }

        public override int ExperienceGauge
        {
            get { return 10 + (int) Math.Pow(Level, 1.5); }
        }

        public override int Power
        {
            get { return 10 + Level*5; }
        }

        private Human()
        {
        }

        public static Human Create()
        {
            var instance = new Human();
            instance.HitPoint = instance.HitGauge;
            instance.MagicPoint = instance.MagicGauge;

            return instance;
        }
    }
}
