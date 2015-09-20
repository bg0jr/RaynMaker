using System;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Describes a series format based on a document which
    /// has a table similar structure (e.g. CSV file).
    /// </summary>
    [Serializable]
    public class SeparatorSeriesFormat : AbstractSeriesFormat
    {
        public SeparatorSeriesFormat( string name )
            : base( name )
        {
            Separator = ";";
        }

        /// <summary>
        /// Creates a deep copy of the given object.
        /// </summary>
        public override IFormat Clone()
        {
            SeparatorSeriesFormat other = new SeparatorSeriesFormat( Datum );
            CloneTo( other );

            other.Separator = Separator;
            other.Anchor = null;
            if ( Anchor != null )
            {
                other.Anchor = Anchor.ForCell( Anchor.Row, Anchor.Column );
            }

            return other;
        }

        /// <summary>
        /// Cell separator used in the file.
        /// </summary>
        public string Separator { get; set; }
    }
}
