using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Plainion.Validation;

namespace RaynMaker.Blade.Entities
{
    [DataContract( Name = "CurrenciesSheet", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    [KnownType( typeof( Currency ) )]
    public class CurrenciesSheet
    {
        public CurrenciesSheet()
        {
            Currencies = new ObservableCollection<Currency>();
        }

        [DataMember]
        [Required, ValidateObject]
        public ObservableCollection<Currency> Currencies { get; private set; }

        [DataMember]
        [Required]
        public int MaxAgeInDays { get; set; }
    }
}
