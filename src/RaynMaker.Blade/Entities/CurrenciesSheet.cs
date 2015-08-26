using System.Collections.ObjectModel;
using Plainion.Validation;
using RaynMaker.Entities;

namespace RaynMaker.Blade.Entities
{
    public class CurrenciesSheet
    {
        public CurrenciesSheet()
        {
            Currencies = new ObservableCollection<Currency>();
        }

        [ValidateObject]
        public ObservableCollection<Currency> Currencies { get; private set; }
    }
}
