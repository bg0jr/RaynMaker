using System;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Describes a series format based on a document which
    /// has a hierarchical structure like XML or HTML documents.
    /// </summary>
    [Serializable]
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "PathSeriesFormat" )]
    public class PathSeriesFormat : AbstractSeriesFormat
    {
        public PathSeriesFormat( string name )
            : base( name )
        {
            ExtractLinkUrl = false;
        }

        /// <summary>
        /// Creates a deep copy of the given object.
        /// </summary>
        public override IFormat Clone()
        {
            PathSeriesFormat other = new PathSeriesFormat( Datum );
            CloneTo( other );

            other.Path = Path;
            other.Anchor = null;
            if ( Anchor != null )
            {
                other.Anchor = Anchor.ForCell( Anchor.Row, Anchor.Column );
            }
            other.ExtractLinkUrl = ExtractLinkUrl;
            other.SeriesName = SeriesName;
            other.InMillions = InMillions;

            return other;
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
