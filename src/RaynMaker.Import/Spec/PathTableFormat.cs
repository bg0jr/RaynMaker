using System;
using System.Linq;

namespace RaynMaker.Import.Spec
{
    [Serializable]
    public class PathTableFormat : AbstractTableFormat
    {
        public PathTableFormat( string name, string path, params FormatColumn[] cols )
            : base( name, cols )
        {
            Path = path;
        }

        public override IFormat Clone()
        {
            PathTableFormat other = new PathTableFormat( Datum, Path, Columns.ToArray() );
            other.SkipRows = SkipRows.ToArray();
            other.SkipColumns = SkipColumns.ToArray();

            return other;
        }

        public string Path { get; private set; }
    }
}
