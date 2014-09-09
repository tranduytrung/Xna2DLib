namespace Dovahkiin.Model.Core
{
    public interface IMovable : ICanvasObject
    {
        /// <summary>
        /// Moving spped in pixel per second
        /// </summary>
        int MovingSpeed { get; }
    }
}