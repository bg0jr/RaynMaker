using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities.Datums
{
    public class InterestExpense : AbstractCurrencyDatum
    {
        [Required]
        public Company Company { get; set; }
    }
}
