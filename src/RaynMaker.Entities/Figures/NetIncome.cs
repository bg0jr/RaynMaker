using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities.Figures
{
    public class NetIncome : AbstractCurrencyFigure
    {
        [Required]
        public Company Company { get; set; }
    }
}
