using System.Collections.Generic;
using System.Linq;
using RaynMaker.Blade.Engine;
using RaynMaker.Entities;

namespace RaynMaker.Blade.Tests.Engine.Fakes
{
    class FakeFigureProviderContext : IFigureProviderContext
    {
        public FakeFigureProviderContext()
        {
            Stock = new Stock();
        }
        
        public Stock Stock { get; private set; }

        public IEnumerable<IDatumSeries> Data
        {
            get { return Enumerable.Empty<IDatumSeries>(); }
        }

        public IDatumSeries GetSeries( string name )
        {
            return DatumSeries.Empty;
        }
    }
}
