using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class Assets : AnualDatum, ICurrencyValue
    {
        [Required]
        public Currency Currency { get; set; }
    }
}
