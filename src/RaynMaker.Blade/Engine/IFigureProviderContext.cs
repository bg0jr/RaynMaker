using System.Collections.Generic;
using RaynMaker.Blade.DataSheetSpec;

namespace RaynMaker.Blade.Engine
{
    public interface IFigureProviderContext
    {
        Asset Asset { get; }

        IEnumerable<T> GetSeries<T>(string name);

        void EnsureCurrencyConsistency( params IEnumerable<ICurrencyDatum>[] values );
    }
}
