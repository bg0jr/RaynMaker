using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class Dividend : DatumWithCurrency
    {
        [Required]
        public int Year { get; set; }
    }
}
