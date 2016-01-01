using System;
using System.Runtime.Serialization;

namespace RaynMaker.Modules.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Describes a figure within one cell of a table.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "PathCellDescriptor" )]
    public class PathCellDescriptor : FigureDescriptorBase
    {
        public PathCellDescriptor( string figure )
            : base( figure )
        {
        }

        /// <summary>
        /// Gets or sets the path within the document to the table.
        /// </summary>
        [DataMember]
        public string Path { get; set; }

        [DataMember]
        public ISeriesLocator Column { get; set; }

        [DataMember]
        public ISeriesLocator Row { get; set; }

        [DataMember]
        public ValueFormat ValueFormat { get; set; }

        [DataMember]
        public string Currency { get; set; }
    }
}
