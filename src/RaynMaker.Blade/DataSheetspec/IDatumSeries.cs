using System;
using System.Collections.Generic;

namespace RaynMaker.Blade.DataSheetSpec
{
    public interface IDatumSeries : IReadOnlyCollection<IDatum>, IFreezable
    {
        string Name { get; }

        Type DatumType { get; }

        Currency Currency { get; }
    }
}
