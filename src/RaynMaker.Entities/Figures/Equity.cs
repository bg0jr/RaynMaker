using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities.Figures
{
    public class Equity : AbstractCurrencyFigure
    {
        [Required]
        public Company Company { get; set; }
    }
}
