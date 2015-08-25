using System.Collections.Generic;
using RaynMaker.Entities;

namespace RaynMaker.Blade.Engine
{
    public interface IFigureProviderContext
    {
        Stock Stock { get; }

        IEnumerable<IDatumSeries> Data { get; }

        IDatumSeries GetSeries( string name );
    }
}
