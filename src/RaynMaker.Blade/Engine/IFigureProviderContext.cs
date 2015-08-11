using System.Collections.Generic;
using RaynMaker.Blade.DataSheetSpec;

namespace RaynMaker.Blade.Engine
{
    public interface IFigureProviderContext
    {
        Asset Asset { get; }

        IDatumSeries GetSeries(string name);
    }
}
