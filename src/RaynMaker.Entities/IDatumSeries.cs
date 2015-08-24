using System;
using System.Collections.Generic;

namespace RaynMaker.Entities
{
    public interface IDatumSeries : IReadOnlyCollection<IDatum>
    {
        string Name { get; }

        Type DatumType { get; }

        Currency Currency { get; }
    }
}
