using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities.Figures
{
    public class CurrentLiabilities : AbstractCurrencyFigure
    {
        [Required]
        public Company Company { get; set; }
    }
}
