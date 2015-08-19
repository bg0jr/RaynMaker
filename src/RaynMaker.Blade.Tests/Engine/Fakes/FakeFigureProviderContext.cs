using RaynMaker.Blade.Engine;
using RaynMaker.Blade.Entities;

namespace RaynMaker.Blade.Tests.Engine.Fakes
{
    class FakeFigureProviderContext : IFigureProviderContext
    {
        public FakeFigureProviderContext()
        {
            Asset = new Stock();
        }
        public Asset Asset { get; private set; }

        public IDatumSeries GetSeries( string name )
        {
            return Series.Empty;
        }
    }
}
