using Dovahkiin.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tranduytrung.Xna.Core;

namespace Dovahkiin.Extension
{
    public static class DrawableObjectExtension
    {
        public static AttachableProperty ModelProperty = 
            AttachableProperty.RegisterProperty(typeof(IMapObject));

        public static IMapObject GetModel(this DrawableObject obj)
        {
            return (IMapObject)obj.GetValue(ModelProperty);
        }

        public static void SetModel(this DrawableObject obj, IMapObject model)
        {
            obj.SetValue(ModelProperty, model);
        }
    }
}
