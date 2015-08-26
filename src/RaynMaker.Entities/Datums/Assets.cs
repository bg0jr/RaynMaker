using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities.Datums
{
    public class Assets : AbstractCurrencyDatum
    {
        [Required]
        public Company Company { get; set; }
    }
}
