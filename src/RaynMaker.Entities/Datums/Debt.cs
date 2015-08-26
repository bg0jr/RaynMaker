using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RaynMaker.Entities.Datums
{
    [DataContract( Name = "Debt", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    public class Debt : AbstractCurrencyDatum
    {
        //[Required]
        public Company Company { get; set; }
    }
}
