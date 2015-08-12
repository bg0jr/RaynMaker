using System;
using System.Collections.Generic;
using RaynMaker.Blade.Entities;

namespace RaynMaker.Blade.DataSheetSpec
{
    public interface IDatumSeries : IReadOnlyCollection<IDatum>, IFreezable
    {
        string Name { get; }

        Type DatumType { get; }

        Currency Currency { get; }
    }
}
