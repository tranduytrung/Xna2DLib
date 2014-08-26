using System;

namespace tranduytrung.DragonCity.Model
{
    public class Dragon : DragonBase
    {
        public override int GoldGeneration
        {
            get { return 100 + (int) (Essence*Level*10); }
        }

        public override long GenerationTime
        {
            get { return 60000 + Level*30000; }
        }

        public override int MaxFoodGauge
        {
            get { return 100 + (int)(Math.Pow(Level, 1.5)*50); }
        }

        public override int SellValue
        {
            get { return 500 + (int) (Math.Sqrt(Level)*50); }
        }

        public static readonly int BuyValue = 2000;
    }
}