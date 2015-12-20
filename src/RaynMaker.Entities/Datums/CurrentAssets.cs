using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities.Datums
{
    public class CurrentAssets : AbstractCurrencyDatum
    {
        [Required]
        public Company Company { get; set; }
    }
}
