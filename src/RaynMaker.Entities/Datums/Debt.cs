using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities.Datums
{
    public class Debt : AbstractCurrencyDatum
    {
        [Required]
        public Company Company { get; set; }
    }
}
