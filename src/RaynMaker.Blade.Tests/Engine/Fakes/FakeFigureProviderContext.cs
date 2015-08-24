using RaynMaker.Blade.Engine;
using RaynMaker.Blade.Entities;

namespace RaynMaker.Blade.Tests.Engine.Fakes
{
    class FakeFigureProviderContext : IFigureProviderContext
    {
        public FakeFigureProviderContext()
        {
            Stock = new Stock();
        }
        public Stock Stock { get; private set; }

        public IDatumSeries GetSeries( string name )
        {
            return DatumSeries.Empty;
        }
    }
}
