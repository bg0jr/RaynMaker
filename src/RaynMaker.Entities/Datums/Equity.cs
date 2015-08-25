using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RaynMaker.Entities.Datums
{
    [DataContract( Name = "Equity", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    public class Equity : AbstractCurrencyDatum
    {
        [Required]
        public Company Company { get; set; }
    }
}
