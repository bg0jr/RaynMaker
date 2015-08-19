using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RaynMaker.Blade.DataSheetSpec
{
    [DataContract( Name = "Reference", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    public class Reference
    {
        [DataMember]
        [Required]
        public Uri Url { get; set; }
    }
}
