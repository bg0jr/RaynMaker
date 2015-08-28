using Blade;

namespace RaynMaker.Import.Spec
{
    public interface IFormat : INamedObject
    {
        IFormat Clone();
    }
}
