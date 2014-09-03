namespace Dovahkiin.Model.Core
{
    public interface IAction
    {
        Actor Source { get; }
        Actor Target { get; }
    }
}
