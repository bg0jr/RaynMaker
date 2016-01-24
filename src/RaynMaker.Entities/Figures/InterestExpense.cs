using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities.Figures
{
    public class InterestExpense : AbstractCurrencyFigure
    {
        [Required]
        public Company Company { get; set; }
    }
}
