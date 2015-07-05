using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class Eps : AnualDatum, ICurrencyDatum
    {
        [Required]
        public Currency Currency { get; set; }
    }
}
