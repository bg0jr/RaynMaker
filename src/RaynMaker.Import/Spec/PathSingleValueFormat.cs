using System;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Describes a single value based on a document which
    /// has a hierarchical structure like XML or HTML documents.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "PathSingleValueFormat" )]
    public class PathSingleValueFormat : AbstractFormat
    {
        public PathSingleValueFormat( string name )
            : base( name )
        {
        }

        [DataMember]
        public string Path { get; set; }

        [DataMember]
        public ValueFormat ValueFormat { get; set; }
    }
}
