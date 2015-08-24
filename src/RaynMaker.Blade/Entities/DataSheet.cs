using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Plainion.Validation;
using RaynMaker.Entities;

namespace RaynMaker.Blade.Entities
{
    [DataContract( Name = "DataSheet", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    [KnownType( typeof( Stock ) )]
    public class DataSheet
    {
        [DataMember( Name = "Asset" )]
        public Stock Stock { get; set; }

        [DataMember]
        [Required, ValidateObject]
        public Company Company { get; set; }
    }
}
