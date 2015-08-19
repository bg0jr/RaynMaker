using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Plainion.Validation;

namespace RaynMaker.Blade.DataSheetSpec
{
    [DataContract( Name = "Stock", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    [KnownType( typeof( Overview ) )]
    public class Stock : Asset
    {
        [DataMember]
        [Required]
        public string Isin { get; set; }

        [DataMember]
        [Required, ValidateObject]
        public Overview Overview { get; set; }
    }
}
