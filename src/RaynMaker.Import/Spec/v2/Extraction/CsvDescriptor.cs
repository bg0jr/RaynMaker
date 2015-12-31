using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Describes extraction of an entire table from CSV formatted document.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "CsvDescriptor" )]
    public class CsvDescriptor : TableDescriptorBase
    {
        public CsvDescriptor( string figure, string sep, params FormatColumn[] cols )
            : base( figure, cols )
        {
            Separator = sep;
        }

        [DataMember]
        public string Separator { get; private set; }
    }
}
