using System;

namespace tranduytrung.DragonCity.Model
{
    public abstract class Building : IMapEntity
    {
        public abstract Type TemplateType { get; }
        public abstract int BuyValue { get; }
        public abstract int SellValue { get; }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }
    }
}