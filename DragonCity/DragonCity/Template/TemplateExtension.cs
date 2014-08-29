using tranduytrung.DragonCity.Template;
using tranduytrung.Xna.Core;

namespace tranduytrung.DragonCity.Template
{
    public static class TemplateExtension
    {
        public static readonly AttachableProperty ContextMenuProperty =
            AttachableProperty.RegisterProperty(typeof (ITemplate));

        public static ITemplate GetTemplate(GameObject obj)
        {
            return (ITemplate)obj.GetValue(ContextMenuProperty);
        }

        public static void SetTemplate(GameObject obj, ITemplate value)
        {
            obj.SetValue(ContextMenuProperty, value);
        }
    }
}
