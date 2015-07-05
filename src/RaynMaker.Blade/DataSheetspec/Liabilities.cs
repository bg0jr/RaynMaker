using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class Liabilities : AnualDatum, ICurrencyDatum
    {
        [Required]
        public Currency Currency { get; set; }
    }
}
