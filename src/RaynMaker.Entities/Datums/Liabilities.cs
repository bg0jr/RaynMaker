using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities.Datums
{
    public class Liabilities : AbstractCurrencyDatum
    {
        [Required]
        public Company Company { get; set; }
    }
}
