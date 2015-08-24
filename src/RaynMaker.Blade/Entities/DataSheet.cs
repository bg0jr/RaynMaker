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
        [Required, ValidateObject]
        public Stock Stock { get; set; }
    }
}
