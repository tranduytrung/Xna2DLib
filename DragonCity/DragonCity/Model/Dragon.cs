using System;
using tranduytrung.DragonCity.Template;

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
            get { return BuyValue/4 + (int) (Math.Sqrt(Level)*50); }
        }

        public override int BuyValue
        {
            get { return 2000; }
        }

        public override Type TemplateType
        {
            get { return typeof(DragonTemplate); }
        }
    }
}