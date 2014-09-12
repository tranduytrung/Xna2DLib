namespace Dovahkiin.Model.Core
{
    public interface ICarriable
    {
        int ResouceId { get; }
        string Name { get; }
        string Description { get; }
        int Value { get; }
    }
}
