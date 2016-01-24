using RaynMaker.Entities;

namespace RaynMaker.Entities
{
    public interface ICurrencyFigure : IFigure
    {
        Currency Currency { get; }
    }
}
