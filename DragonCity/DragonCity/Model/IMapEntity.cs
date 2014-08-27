using System;

namespace tranduytrung.DragonCity.Model
{
    public interface IMapEntity : ICloneable
    {
        Type TemplateType { get; }
    }
}
