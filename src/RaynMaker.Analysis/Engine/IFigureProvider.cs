
namespace RaynMaker.Analysis.Engine
{
    public interface IFigureProvider
    {
        string Name { get; }

        object ProvideValue( IFigureProviderContext context );
    }
}
