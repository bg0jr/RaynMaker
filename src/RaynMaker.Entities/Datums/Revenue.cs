using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities.Datums
{
    public class Revenue : AbstractCurrencyDatum
    {
        [Required]
        public Company Company { get; set; }
    }
}
