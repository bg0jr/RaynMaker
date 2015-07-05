using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class Revenue : AnualDatum, ICurrencyDatum
    {
        [Required]
        public Currency Currency { get; set; }
    }
}
