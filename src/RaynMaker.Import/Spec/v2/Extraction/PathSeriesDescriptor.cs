using System;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Describes a series format based on a document which
    /// has a hierarchical structure like XML or HTML documents.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "PathSeriesFormat" )]
    public class PathSeriesDescriptor : AbstractSeriesDescriptor
    {
        public PathSeriesDescriptor( string datum )
            : base( datum )
        {
        }

        /// <summary>
        /// Path which describes the position of the series.
        /// </summary>
        [DataMember]
        public string Path { get; set; }
    }
}
