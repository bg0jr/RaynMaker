using System;
using System.Linq;

namespace RaynMaker.Import.Spec
{
    [Serializable]
    public class CsvFormat : AbstractTableFormat
    {
        public CsvFormat( string name, string sep, params FormatColumn[] cols )
            : base( name, cols )
        {
            Separator = sep;
        }

        public override IFormat Clone()
        {
            CsvFormat other = new CsvFormat( Name, Separator, Columns.ToArray() );
            other.SkipRows = SkipRows.ToArray();
            other.SkipColumns = SkipColumns.ToArray();

            return other;
        }

        public string Separator { get; private set; }
    }
}
