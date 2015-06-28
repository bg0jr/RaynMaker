using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class Revenue : AnualDatum, ICurrencyValue
    {
        [Required]
        public Currency Currency { get; set; }
    }
}
