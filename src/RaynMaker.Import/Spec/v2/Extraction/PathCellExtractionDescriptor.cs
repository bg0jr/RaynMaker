using System;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec.v2.Extraction
{
    // TODO: of course this class should not derive from PathSeriesFormat as it is no series format. this is only a workaround to reuse all the anchor related parser logic
    // until the parser refactoring is done. In this context we sould check whether PathSeriesFormat should go for anchor only. and then decide about the design of this class
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "PathCellFormat" )]
    public class PathCellExtractionDescriptor : PathSeriesExtractionDescriptor
    {
        public PathCellExtractionDescriptor( string datum )
            : base( datum )
        {
            Expand = CellDimension.None;
            Anchor = null;
        }

        [DataMember]
        public string Currency { get; set; }
    }
}
