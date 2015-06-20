using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.Sdk
{
    public class DatumWithCurrency : Datum<double>
    {
        [Required]
        public Currency Curreny { get; set; }
    }
}
