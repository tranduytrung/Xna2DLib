namespace tranduytrung.Xna.Core
{
    public delegate void SetAccessorDelegate<in T>(T value);
    public delegate T GetAccessorDelegate<out T>();
}
