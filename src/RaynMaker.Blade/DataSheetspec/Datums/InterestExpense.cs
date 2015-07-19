using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec.Datums
{
    public class InterestExpense : AnualDatum, ICurrencyDatum
    {
        [Required]
        public Currency Currency { get; set; }
    }
}
