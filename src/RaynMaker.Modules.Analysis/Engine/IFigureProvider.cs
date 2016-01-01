
namespace RaynMaker.Modules.Analysis.Engine
{
    public interface IFigureProvider
    {
        string Name { get; }

        object ProvideValue( IFigureProviderContext context );
    }
}
