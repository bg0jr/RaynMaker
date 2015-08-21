using RaynMaker.Blade.Entities;
using RaynMaker.Entities;

namespace RaynMaker.Blade.Entities
{
    public interface ICurrencyDatum : IDatum
    {
        Currency Currency { get; }
    }
}
