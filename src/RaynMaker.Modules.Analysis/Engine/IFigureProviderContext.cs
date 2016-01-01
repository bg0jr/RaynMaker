using System.Collections.Generic;
using RaynMaker.Entities;

namespace RaynMaker.Modules.Analysis.Engine
{
    public interface IFigureProviderContext
    {
        Stock Stock { get; }

        IEnumerable<IDatumSeries> Data { get; }

        IDatumSeries GetSeries( string name );

        double TranslateCurrency( double value, Currency source, Currency target );
  }
}
