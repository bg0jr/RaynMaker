using System.Collections.Generic;
using RaynMaker.Entities;

namespace RaynMaker.Analysis.Engine
{
    public interface IFigureProviderContext
    {
        Stock Stock { get; }

        IEnumerable<IDatumSeries> Data { get; }

        IDatumSeries GetSeries( string name );
    }
}
