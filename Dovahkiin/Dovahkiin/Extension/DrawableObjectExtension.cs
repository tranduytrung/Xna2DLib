using Dovahkiin.Model.Core;
using tranduytrung.Xna.Core;

namespace Dovahkiin.Extension
{
    public static class DrawableObjectExtension
    {
        public static AttachableProperty MapObjectModelProperty = 
            AttachableProperty.RegisterProperty(typeof(IMapObject));

        public static AttachableProperty CanvasObjectModelProperty =
            AttachableProperty.RegisterProperty(typeof(ICanvasObject));

        public static IMapObject GetMapObjectModel(this DrawableObject obj)
        {
            return (IMapObject)obj.GetValue(CanvasObjectModelProperty);
        }

        public static void SetMapObjectModel(this DrawableObject obj, IMapObject model)
        {
            obj.SetValue(CanvasObjectModelProperty, model);
        }

        public static ICanvasObject GetCanvasObjectModel(this DrawableObject obj)
        {
            return (ICanvasObject)obj.GetValue(CanvasObjectModelProperty);
        }

        public static void SetCanvasObjectModel(this DrawableObject obj, ICanvasObject model)
        {
            obj.SetValue(CanvasObjectModelProperty, model);
        }
    }
}
