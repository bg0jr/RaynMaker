using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities.Datums
{
    public class EBIT : AbstractCurrencyDatum
    {
        [Required]
        public Company Company { get; set; }
    }
}
