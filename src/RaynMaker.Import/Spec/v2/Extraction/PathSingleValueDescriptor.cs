using System;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Describes a single value based on a document which
    /// has a hierarchical structure like XML or HTML documents.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "PathSingleValueFormat" )]
    public class PathSingleValueDescriptor : AbstractFigureDescriptor
    {
        public PathSingleValueDescriptor( string datum )
            : base( datum )
        {
        }

        [DataMember]
        public string Path { get; set; }

        [DataMember]
        public ValueFormat ValueFormat { get; set; }
    }
}
