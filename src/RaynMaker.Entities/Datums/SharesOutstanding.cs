using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities.Datums
{
    public class SharesOutstanding : AbstractDatum
    {
        [Required]
        public Company Company { get; set; }
    }
}
