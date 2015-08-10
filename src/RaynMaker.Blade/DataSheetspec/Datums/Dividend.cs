using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec.Datums
{
    /// <summary>
    /// Total dividend payment. NOT per share
    /// </summary>
    public class Dividend : AnualDatum, ICurrencyDatum
    {
        [Required]
        public Currency Currency { get; set; }
    }
}
