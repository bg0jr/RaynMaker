using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class AnualDatum : Datum
    {
        [Required]
        public int Year { get; set; }
    }
}
