using RaynMaker.Entities;

namespace RaynMaker.Entities
{
    public interface ICurrencyDatum : IDatum
    {
        Currency Currency { get; }
    }
}
