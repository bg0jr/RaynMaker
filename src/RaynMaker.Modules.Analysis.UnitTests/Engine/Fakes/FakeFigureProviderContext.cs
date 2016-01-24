using System.Collections.Generic;
using System.Linq;
using RaynMaker.Modules.Analysis.Engine;
using RaynMaker.Entities;

namespace RaynMaker.Modules.Analysis.UnitTests.Engine.Fakes
{
    class FakeFigureProviderContext : IFigureProviderContext
    {
        public FakeFigureProviderContext()
        {
            Stock = new Stock();
        }
        
        public Stock Stock { get; private set; }

        public IEnumerable<IFigureSeries> Data
        {
            get { return Enumerable.Empty<IFigureSeries>(); }
        }

        public IFigureSeries GetSeries( string name )
        {
            return FigureSeries.Empty;
        }

        public double TranslateCurrency( double value, Currency source, Currency target )
        {
            return value;
        }
    }
}
