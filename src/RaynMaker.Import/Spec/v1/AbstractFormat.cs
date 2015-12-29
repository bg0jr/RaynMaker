using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec.v1
{
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "AbstractFormat" )]
    public abstract class AbstractFormat : IFormat
    {
        protected AbstractFormat( string datum )
        {
            Datum = datum;
        }

        [Required]
        [DataMember]
        public string Datum { get; set; }
    
        [DataMember]
        public bool InMillions { get; set; }
    }
}
