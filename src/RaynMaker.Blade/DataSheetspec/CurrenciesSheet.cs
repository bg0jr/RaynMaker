using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
            Currencies = new List<Currency>();
        }

        [Required, ValidateObject]
        public IList<Currency> Currencies { get; private set; }

        [Required]
        public int MaxAgeInDays { get; set; }
    }
}
