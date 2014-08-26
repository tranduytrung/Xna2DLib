using System;

namespace tranduytrung.DragonCity.Model
{
    public abstract class DragonBase
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
