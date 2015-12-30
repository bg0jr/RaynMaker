using System;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Describes a series format based on a document which
    /// has a table similar structure (e.g. CSV file).
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "SeparatorSeriesFormat" )]
    public class SeparatorSeriesDescriptor : AbstractSeriesDescriptor
    {
        public SeparatorSeriesDescriptor( string datum )
            : base( datum )
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
