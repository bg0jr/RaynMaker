using System;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Describes a series format based on a document which
    /// has a hierarchical structure like XML or HTML documents.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "PathSeriesFormat" )]
    public class PathSeriesExtractionDescriptor : AbstractSeriesExtractionDescriptor
    {
        public PathSeriesExtractionDescriptor( string datum )
            : base( datum )
        {
            ExtractLinkUrl = false;
        }

        /// <summary>
        /// Path which describes the position of the series.
        /// </summary>
        [DataMember]
        public string Path { get; set; }

        /// <summary>
        /// In case of a document that supports link extraction below the actual value this property
        /// indicates that the link should be extracted instead of the display text.
        /// </summary>
        [DataMember]
        public bool ExtractLinkUrl { get; set; }

        [DataMember]
        public string SeriesName { get; set; }
    }
}
