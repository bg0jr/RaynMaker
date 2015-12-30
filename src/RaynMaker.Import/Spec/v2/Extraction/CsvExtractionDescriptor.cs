using System;
using System.Linq;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec.v2.Extraction
{
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "CsvFormat" )]
    public class CsvExtractionDescriptor : AbstractTableExtractionDescriptor
    {
        public CsvExtractionDescriptor( string datum, string sep, params FormatColumn[] cols )
            : base( datum, cols )
        {
            Separator = sep;
        }

        [DataMember]
        public string Separator { get; private set; }
    }
}
