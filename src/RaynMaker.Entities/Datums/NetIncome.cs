using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities.Datums
{
    public class NetIncome : AbstractCurrencyDatum
    {
        [Required]
        public Company Company { get; set; }
    }
}
