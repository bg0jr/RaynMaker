using System;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Describes a series format based on a document which
    /// has a hierarchical structure like XML or HTML documents.
    /// </summary>
    [Serializable]
    public class PathSeriesFormat : AbstractSeriesFormat
    {
        public PathSeriesFormat( string name )
            : base( name )
        {
            ExtractLinkUrl = false;
        }

        //public PathSeriesFormat( PathSeriesFormat format, params TransformAction[] rules )
        //    : base( format, rules )
        //{
        //    Path = rules.ApplyTo<string>( () => format.Path );
        //    Anchor = rules.ApplyTo<Anchor>( () => format.Anchor );
        //}

        /// <summary>
        /// Creates a deep copy of the given object.
        /// </summary>
        public override IFormat Clone()
        {
            PathSeriesFormat other = new PathSeriesFormat( Name );
            CloneTo( other );

            other.Path = Path;
            other.Anchor = null;
            if ( Anchor != null )
            {
                other.Anchor = Anchor.ForCell( Anchor.Row, Anchor.Column );
            }

            return other;
        }

        /// <summary>
        /// Path which describes the position of the series.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// In case of a document that supports link extraction below the actual value this property
        /// indicates that the link should be extracted instead of the display text.
        /// </summary>
        public bool ExtractLinkUrl { get; set; }
    }
}
