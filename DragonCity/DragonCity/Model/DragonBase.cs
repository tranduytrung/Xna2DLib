using System;

namespace tranduytrung.DragonCity.Model
{
    public abstract class DragonBase : IMapEntity
    {
        protected static readonly Random RandomInstance = new Random();
        public string Name { get; set; }
        public int Level { get; private set; }
        public int AccumulatedFood { get; private set;}
        public double Essence { get; private set; }
        public abstract int GoldGeneration { get; }
        public abstract long GenerationTime { get; }
        public abstract int MaxFoodGauge { get; }
        public abstract int SellValue { get; }
        public abstract int BuyValue { get; }

        public virtual object Clone()
        {
            var obj = (DragonBase) MemberwiseClone();
            obj.Essence = RandomInstance.NextDouble();
            return obj;
        }

        public abstract Type TemplateType { get; }

        public void Feed(int food)
        {
            AccumulatedFood += food;
            while (AccumulatedFood < MaxFoodGauge)
            {
                ++Level;
                AccumulatedFood -= MaxFoodGauge;
            }
        }

        protected DragonBase()
        {
            Essence = RandomInstance.NextDouble();
        }
    }
}
