using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities.Datums
{
    public class CurrentLiabilities : AbstractCurrencyDatum
    {
        [Required]
        public Company Company { get; set; }
    }
}
