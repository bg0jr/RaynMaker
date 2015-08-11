using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class CurrencyDatum : Datum, ICurrencyDatum
    {
        [Required]
        public Currency Currency { get; set; }
    }
}
