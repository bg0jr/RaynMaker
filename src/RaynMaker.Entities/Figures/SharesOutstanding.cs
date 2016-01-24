using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities.Figures
{
    public class SharesOutstanding : AbstractFigure
    {
        [Required]
        public Company Company { get; set; }
    }
}
