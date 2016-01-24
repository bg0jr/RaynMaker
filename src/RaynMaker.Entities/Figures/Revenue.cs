using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities.Figures
{
    public class Revenue : AbstractCurrencyFigure
    {
        [Required]
        public Company Company { get; set; }
    }
}
