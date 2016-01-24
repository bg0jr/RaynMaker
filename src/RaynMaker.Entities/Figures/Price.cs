using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities.Figures
{
    public class Price : AbstractCurrencyFigure
    {
        [Required]
        public Stock Stock { get; set; }
    }
}
