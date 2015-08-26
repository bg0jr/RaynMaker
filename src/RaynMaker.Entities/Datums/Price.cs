using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities.Datums
{
    public class Price : AbstractCurrencyDatum
    {
        [Required]
        public Stock Stock { get; set; }
    }
}
