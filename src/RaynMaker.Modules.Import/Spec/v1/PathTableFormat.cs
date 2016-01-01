using System;
using System.Linq;
using System.Runtime.Serialization;

namespace RaynMaker.Modules.Import.Spec.v1
{
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "PathTableFormat" )]
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
