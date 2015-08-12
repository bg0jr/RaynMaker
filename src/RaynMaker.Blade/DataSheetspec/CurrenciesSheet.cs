using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Markup;
using Plainion.Validation;

namespace RaynMaker.Blade.DataSheetSpec
{
    [DefaultProperty( "Currencies" ), ContentProperty( "Currencies" )]
    public class CurrenciesSheet : DataTemplate
    {
        public CurrenciesSheet()
        {
            Currencies = new ObservableCollection<Currency>();
        }

        [Required, ValidateObject]
        public ObservableCollection<Currency> Currencies { get; private set; }

        [Required]
        public int MaxAgeInDays { get; set; }
    }
}
