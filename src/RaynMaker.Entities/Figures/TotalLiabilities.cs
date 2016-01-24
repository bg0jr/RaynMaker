using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities.Figures
{
    public class TotalLiabilities : AbstractCurrencyFigure
    {
        [Required]
        public Company Company { get; set; }
    }
}
