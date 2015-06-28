using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class Dept : AnualDatum, ICurrencyValue
    {
        [Required]
        public Currency Currency { get; set; }
    }
}
