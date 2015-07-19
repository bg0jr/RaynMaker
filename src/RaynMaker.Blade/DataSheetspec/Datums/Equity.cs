using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec.Datums
{
    public class Equity : AnualDatum, ICurrencyDatum
    {
        [Required]
        public Currency Currency { get; set; }
    }
}
