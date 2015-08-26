using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RaynMaker.Entities.Datums
{
    [DataContract( Name = "SharesOutstanding", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    public class SharesOutstanding : AbstractDatum
    {
        //[Required]
        public Stock Stock { get; set; }
    }
}
