using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities.Datums
{
    /// <summary>
    /// Total dividend payment. NOT per share
    /// </summary>
    public class Dividend : AbstractCurrencyDatum
    {
        [Required]
        public Company Company { get; set; }
    }
}
