using System;
using System.Collections.Generic;
using RaynMaker.Blade.Entities;
using RaynMaker.Entities;

namespace RaynMaker.Blade.Entities
{
    public interface IDatumSeries : IReadOnlyCollection<IDatum>
    {
        string Name { get; }

        Type DatumType { get; }

        Currency Currency { get; }
    }
}
