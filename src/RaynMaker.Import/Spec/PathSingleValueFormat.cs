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

        public override IFormat Clone()
        {
            var other = new PathSingleValueFormat( Datum );

            other.Path = Path;
            other.ValueFormat = new ValueFormat( ValueFormat );

            return other;
        }

        public string Path { get; set; }

        public ValueFormat ValueFormat { get; set; }
    }
}
