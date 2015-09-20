using System;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Describes a single value based on a document which
    /// has a hierarchical structure like XML or HTML documents.
    /// </summary>
    [Serializable]
    public class PathSingleValueFormat : AbstractFormat
    {
        public PathSingleValueFormat( string name )
            : base( name )
        {
        }

        /// <summary>
        /// Creates a deep copy of the given object.
        /// </summary>
        public override IFormat Clone()
        {
            var other = new PathSingleValueFormat( Datum );

            other.Path = Path;
            other.ValueFormat = new ValueFormat( ValueFormat );

            return other;
        }

        /// <summary>
        /// Path to the value.
        /// </summary>
        public string Path { get; set; }

        /// <summary/>
        public ValueFormat ValueFormat { get; set; }
    }
}
