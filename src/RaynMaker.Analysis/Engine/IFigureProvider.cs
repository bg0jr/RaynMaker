
namespace RaynMaker.Blade.Engine
{
    public interface IFigureProvider
    {
        string Name { get; }

        object ProvideValue( IFigureProviderContext context );
    }
}
