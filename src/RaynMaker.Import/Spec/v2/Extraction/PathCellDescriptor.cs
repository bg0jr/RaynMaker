using System;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec.v2.Extraction
{
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "PathCellFormat" )]
    public class PathCellDescriptor : AbstractFigureDescriptor
    {
        public PathCellDescriptor( string datum )
            : base( datum )
        {
        }

        /// <summary>
        /// Path which describes the position of the series.
        /// </summary>
        [DataMember]
        public string Path { get; set; }

        [DataMember]
        public ITablePositionLocator Column { get; set; }

        [DataMember]
        public ITablePositionLocator Row { get; set; }

        [DataMember]
        public FormatColumn ValueFormat { get; set; }

        [DataMember]
        public string Currency { get; set; }
    }
}
