using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using Plainion.Validation;

namespace RaynMaker.Blade.Entities
{
    [DataContract( Name = "DataSheet", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    [KnownType( typeof( Asset ) ), KnownType( typeof( Stock ) )]
    public class DataSheet
    {
        [DataMember]
        [Required, ValidateObject]
        public Asset Asset { get; set; }
    }
}
