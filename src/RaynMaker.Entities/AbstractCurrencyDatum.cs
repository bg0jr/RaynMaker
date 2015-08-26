using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities
{
    public abstract class AbstractCurrencyDatum : AbstractDatum, ICurrencyDatum
    {
        private Currency myCurrency;

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
