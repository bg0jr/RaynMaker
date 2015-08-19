using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using RaynMaker.Blade.Entities;

namespace RaynMaker.Blade.Entities
{
    [DataContract( Name = "CurrencyDatum", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    [KnownType( typeof( Currency ) )]
    public class CurrencyDatum : Datum, ICurrencyDatum
    {
        private Currency myCurrency;

        [DataMember]
        [Required]
        public Currency Currency
        {
            get { return myCurrency; }
            set { SetProperty( ref myCurrency, value ); }
        }
    }
}
