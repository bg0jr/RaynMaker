using System;
using System.Runtime.Serialization;

namespace RaynMaker.Modules.Import.Spec.v1
{
    /// <summary>
    /// Describes a single value based on a document which
    /// has a hierarchical structure like XML or HTML documents.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "PathSingleValueFormat" )]
    class PathSingleValueFormat : AbstractFormat
    {
        public PathSingleValueFormat( string datum )
            : base( datum )
        {
        }

        [DataMember]
        public string Path { get; set; }

        [DataMember]
        public ValueFormat ValueFormat { get; set; }
    }
}
