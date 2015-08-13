using RaynMaker.Blade.Entities;

namespace RaynMaker.Blade.Entities
{
    public interface ICurrencyDatum : IDatum
    {
        Currency Currency { get; }
    }
}
