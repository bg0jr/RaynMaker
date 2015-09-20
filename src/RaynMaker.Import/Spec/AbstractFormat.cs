using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec
{
    [Serializable]
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "AbstractFormat" )]
    public abstract class AbstractFormat : IFormat
    {
        protected AbstractFormat( string datum )
        {
            Datum = datum;
        }

        public abstract IFormat Clone();

        [Required]
        [DataMember]
        public string Datum { get; set; }
    }
}
