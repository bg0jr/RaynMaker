using RaynMaker.Blade.Entities;

namespace RaynMaker.Blade.DataSheetSpec
{
    public interface ICurrencyDatum : IDatum
    {
        Currency Currency { get; }
    }
}
