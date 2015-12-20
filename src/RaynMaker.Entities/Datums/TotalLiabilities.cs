using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities.Datums
{
    public class TotalLiabilities : AbstractCurrencyDatum
    {
        [Required]
        public Company Company { get; set; }
    }
}
