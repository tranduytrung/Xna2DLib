using System;
using tranduytrung.DragonCity.Template;

namespace tranduytrung.DragonCity.Model
{
    public class Habitat : Building
    {
        public override Type TemplateType
        {
            get { return typeof(HabitatTemplate); }
        }

        public override int BuyValue
        {
            get { return 1993; }
        }

        public override int SellValue
        {
            get { return BuyValue/4; }
        }
    }
}