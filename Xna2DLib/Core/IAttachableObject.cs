
namespace tranduytrung.Xna.Core
{
    public interface IAttachableObject
    {
        object GetValue(AttachableProperty property);
        void SetValue(AttachableProperty property, object value);
    }
}
