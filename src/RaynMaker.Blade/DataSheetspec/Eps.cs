using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class Eps : DatumWithCurrency
    {
        [Required]
        public int Year { get; set; }
    }
}
