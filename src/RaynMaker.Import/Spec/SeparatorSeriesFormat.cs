using System;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Describes a series format based on a document which
    /// has a table similar structure (e.g. CSV file).
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "SeparatorSeriesFormat" )]
    public class SeparatorSeriesFormat : AbstractSeriesFormat
    {
        public SeparatorSeriesFormat( string name )
            : base( name )
        {
            Separator = ";";
        }

        /// <summary>
        /// Cell separator used in the file.
        /// </summary>
        [DataMember]
        public string Separator { get; set; }
    }
}
