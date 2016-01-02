using System;
using System.Linq;
using System.Runtime.Serialization;

namespace RaynMaker.Modules.Import.Spec.v1
{
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "CsvFormat" )]
    class CsvFormat : AbstractTableFormat
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
