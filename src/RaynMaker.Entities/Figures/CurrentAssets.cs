using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities.Figures
{
    public class CurrentAssets : AbstractCurrencyFigure
    {
        [Required]
        public Company Company { get; set; }
    }
}
