using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class EBIT : AnualDatum, ICurrencyValue
    {
        [Required]
        public Currency Currency { get; set; }
    }
}
