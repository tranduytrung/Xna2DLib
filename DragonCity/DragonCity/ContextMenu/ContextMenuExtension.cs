using tranduytrung.Xna.Core;

namespace tranduytrung.DragonCity.ContextMenu
{
    public static class ContextMenuExtension
    {
        public static readonly AttachableProperty ContextMenuProperty =
            AttachableProperty.RegisterProperty(typeof (DrawableObject));

        public static DrawableObject GetContextMenu(GameObject obj)
        {
            return (DrawableObject)obj.GetValue(ContextMenuProperty);
        }

        public static void SetContextMenu(GameObject obj, DrawableObject value)
        {
            obj.SetValue(ContextMenuProperty, value);
        }
    }
}
