using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class NetIncome : AnualDatum, ICurrencyDatum
    {
        [Required]
        public Currency Currency { get; set; }
    }
}
