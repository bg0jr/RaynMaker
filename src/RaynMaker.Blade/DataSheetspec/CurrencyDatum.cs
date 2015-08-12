using System.ComponentModel.DataAnnotations;
using RaynMaker.Blade.Entities;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class CurrencyDatum : Datum, ICurrencyDatum
    {
        [Required]
        public Currency Currency { get; set; }
    }
}
