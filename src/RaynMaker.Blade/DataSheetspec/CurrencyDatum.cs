using System.ComponentModel.DataAnnotations;
using RaynMaker.Blade.Entities;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class CurrencyDatum : Datum, ICurrencyDatum
    {
        private Currency myCurrency;

        [Required]
        public Currency Currency
        {
            get { return myCurrency; }
            set { SetProperty( ref myCurrency, value ); }
        }
    }
}
