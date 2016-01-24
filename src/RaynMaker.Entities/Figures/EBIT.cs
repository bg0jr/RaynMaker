using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities.Figures
{
    public class EBIT : AbstractCurrencyFigure
    {
        [Required]
        public Company Company { get; set; }
    }
}
