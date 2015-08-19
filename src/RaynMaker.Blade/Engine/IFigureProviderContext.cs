using RaynMaker.Blade.Entities;

namespace RaynMaker.Blade.Engine
{
    public interface IFigureProviderContext
    {
        Asset Asset { get; }

        IDatumSeries GetSeries( string name );
    }
}
