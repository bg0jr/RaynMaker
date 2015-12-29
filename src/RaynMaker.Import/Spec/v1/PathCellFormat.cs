using System;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec.v1
{
    // TODO: of course this class should not derive from PathSeriesFormat as it is no series format. this is only a workaround to reuse all the anchor related parser logic
    // until the parser refactoring is done. In this context we sould check whether PathSeriesFormat should go for anchor only. and then decide about the design of this class
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "PathCellFormat" )]
    public class PathCellFormat : PathSeriesFormat
    {
        public PathCellFormat( string datum )
            : base( datum )
        {
            Expand = CellDimension.None;
            Anchor = null;
        }

        [DataMember]
        public string Currency { get; set; }
    }
}
