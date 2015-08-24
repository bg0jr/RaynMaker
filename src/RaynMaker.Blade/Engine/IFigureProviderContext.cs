using RaynMaker.Blade.Entities;

namespace RaynMaker.Blade.Engine
{
    public interface IFigureProviderContext
    {
        Stock Stock { get; }

        IDatumSeries GetSeries( string name );
    }
}
