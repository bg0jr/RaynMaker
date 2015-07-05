using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class Assets : AnualDatum, ICurrencyDatum
    {
        [Required]
        public Currency Currency { get; set; }
    }
}
