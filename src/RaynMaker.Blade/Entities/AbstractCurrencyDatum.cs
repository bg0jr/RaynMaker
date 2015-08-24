using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using RaynMaker.Entities;

namespace RaynMaker.Blade.Entities
{
    [DataContract( Name = "CurrencyDatum", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    [KnownType( typeof( Currency ) )]
    public abstract class AbstractCurrencyDatum : AbstractDatum, ICurrencyDatum
    {
        private Currency myCurrency;

        [DataMember]
        [Required]
        public Currency Currency
        {
            get { return myCurrency; }
            set
            {
                if( SetProperty( ref myCurrency, value ) )
                {
                    UpdateTimestamp();
                }
            }
        }
    }
}
