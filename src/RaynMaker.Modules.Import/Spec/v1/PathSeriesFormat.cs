using System;
using System.Runtime.Serialization;

namespace RaynMaker.Modules.Import.Spec.v1
{
    /// <summary>
    /// Describes a series format based on a document which
    /// has a hierarchical structure like XML or HTML documents.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "PathSeriesFormat" )]
    public class PathSeriesFormat : AbstractSeriesFormat
    {
        public PathSeriesFormat( string datum )
            : base( datum )
        {
        }

        /// <summary>
        /// Path which describes the position of the series.
        /// </summary>
        [DataMember]
        public string Path { get; set; }

        [DataMember]
        public string SeriesName { get; set; }
    }
}
