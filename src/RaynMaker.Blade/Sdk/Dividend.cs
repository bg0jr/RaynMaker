using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.Sdk
{
    public class Dividend : DatumWithCurrency
    {
        [Required]
        public int Year { get; set; }
    }
}
