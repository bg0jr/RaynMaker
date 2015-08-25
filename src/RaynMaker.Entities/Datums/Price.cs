using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RaynMaker.Entities.Datums
{
    [DataContract( Name = "Price", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    public class Price : AbstractCurrencyDatum
    {
        [IgnoreDataMember]
        [Required]
        public Stock Stock { get; set; }
    }
}
