using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities.Figures
{
    /// <summary>
    /// Total dividend payment. NOT per share
    /// </summary>
    public class Dividend : AbstractCurrencyFigure
    {
        [Required]
        public Company Company { get; set; }
    }
}
