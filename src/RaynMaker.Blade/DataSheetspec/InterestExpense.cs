using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class InterestExpense : AnualDatum, ICurrencyValue
    {
        [Required]
        public Currency Currency { get; set; }
    }
}
