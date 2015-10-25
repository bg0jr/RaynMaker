using RaynMaker.Entities;

namespace RaynMaker.Infrastructure.Services
{
    public interface ICurrencyTranslationRateProvider
    {
        double GetRate( Currency source, Currency target );
    }
}
