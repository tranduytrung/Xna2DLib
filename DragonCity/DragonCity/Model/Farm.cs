using System;
using tranduytrung.DragonCity.Template;

namespace tranduytrung.DragonCity.Model
{
    public class Farm : Building
    {
        public override Type TemplateType
        {
            get { return typeof(FarmTemplate); }
        }

        public override int BuyValue
        {
            get { return 1000; }
        }

        public override int SellValue
        {
            get { return BuyValue/4; }
        }

        public int FoodGeneration { get { return 100; }}
        public TimeSpan GenerationTime { get { return TimeSpan.FromSeconds(60); } }
    }
}