using System.Collections.ObjectModel;
using System.ComponentModel;
using RaynMaker.Entities;

namespace RaynMaker.Infrastructure.Services
{
    public interface ICurrenciesLut : INotifyPropertyChanged
    {
        int MaxCurrencyTranslationsAgeInDays { get; set; }

        ObservableCollection<Currency> Currencies { get; }

        void Reload();
        void Save();
    }
}
