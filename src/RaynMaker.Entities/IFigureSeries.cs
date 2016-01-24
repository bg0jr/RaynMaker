using System;
using System.Collections.Generic;

namespace RaynMaker.Entities
{
    public interface IFigureSeries : IReadOnlyCollection<IFigure>
    {
        string Name { get; }

        Type FigureType { get; }

        Currency Currency { get; }
    }
}
