using System;
using System.Linq;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec
{
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "PathTableFormat" )]
    public class PathTableFormat : AbstractTableFormat
    {
        public PathTableFormat( string name, string path, params FormatColumn[] cols )
            : base( name, cols )
        {
            Path = path;
        }

        [DataMember]
        public string Path { get; private set; }
    }
}
