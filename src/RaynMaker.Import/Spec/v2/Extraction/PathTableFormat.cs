using System;
using System.Linq;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec.v2.Extraction
{
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "PathTableFormat" )]
    public class PathTableFormat : AbstractTableFormat
    {
        public PathTableFormat( string datum, string path, params FormatColumn[] cols )
            : base( datum, cols )
        {
            Path = path;
        }

        [DataMember]
        public string Path { get; private set; }
    }
}
