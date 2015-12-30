using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using RaynMaker.Import.Parsers;

namespace RaynMaker.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Base class for all formats that describe a series of data.
    /// A series consists of a set of time-value pairs.
    /// </summary>
    // TODO: actually we no longer need "expand" if we have an anchor
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "AbstractSeriesFormat" )]
    public abstract class AbstractSeriesFormat : AbstractDimensionalFormat
    {
        protected AbstractSeriesFormat( string name )
            : base( name )
        {
            SeriesNamePosition = -1;
            TimeAxisPosition = -1;
        }

        /// <summary>
        /// Direction of the series in the "table".
        /// </summary>
        [DataMember]
        public CellDimension Expand { get; set; }

        /// <summary>
        /// Position of the series name: the column
        /// if Expand == Row, the row otherwise.
        /// </summary>
        [DataMember]
        public int SeriesNamePosition { get; set; }

        /// <summary>
        /// Position of the time axis: the row
        /// if Expand == Row, the column otherwise.
        /// </summary>
        [DataMember]
        public int TimeAxisPosition { get; set; }

        /// <summary>
        /// Format of the value column.
        /// </summary>
        [DataMember]
        public FormatColumn ValueFormat { get; set; }

        /// <summary>
        /// Format of the time axis column.
        /// </summary>
        [DataMember]
        public FormatColumn TimeAxisFormat { get; set; }

        /// <summary>
        /// Defines how to find the position of the series in the table.
        /// </summary>
        [DataMember]
        public Anchor Anchor { get; set; }
    }
}
