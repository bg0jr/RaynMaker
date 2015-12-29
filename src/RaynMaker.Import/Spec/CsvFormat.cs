using System;
using System.Linq;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec
{
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "CsvFormat" )]
    public class CsvFormat : AbstractTableFormat
    {
        public CsvFormat( string datum, string sep, params FormatColumn[] cols )
            : base( datum, cols )
        {
            Separator = sep;
        }

        [DataMember]
        public string Separator { get; private set; }
    }
}
