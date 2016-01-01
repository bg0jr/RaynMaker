using System;
using System.Runtime.Serialization;

namespace RaynMaker.Modules.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Describes a figure within a document which can be directly located using an explicit path.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "PathSingleValueDescriptor" )]
    public class PathSingleValueDescriptor : FigureDescriptorBase
    {
        public PathSingleValueDescriptor( string figure )
            : base( figure )
        {
        }

        /// <summary>
        /// Gets or sets the path within the document to the figure.
        /// </summary>
        [DataMember]
        public string Path { get; set; }

        [DataMember]
        public ValueFormat ValueFormat { get; set; }
    }
}
