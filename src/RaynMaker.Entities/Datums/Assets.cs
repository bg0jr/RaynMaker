using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RaynMaker.Entities.Datums
{
    [DataContract( Name = "Assets", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    public class Assets : AbstractCurrencyDatum
    {
        [Required]
        public Company Company { get; set; }
    }
}
