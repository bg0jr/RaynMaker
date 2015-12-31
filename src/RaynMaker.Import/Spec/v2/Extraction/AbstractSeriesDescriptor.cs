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
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "AbstractSeriesFormat" )]
    public abstract class AbstractSeriesDescriptor : AbstractDimensionalDescriptor
    {
        protected AbstractSeriesDescriptor( string name )
            : base( name )
        {
            TimeAxisPosition = -1;
        }

        /// <summary>
        /// Defines how to find the position of the series in the table.
        /// </summary>
        [DataMember]
        public TableFragmentDescriptor Anchor { get; set; }
        
        /// <summary>
        /// Position of the time axis series. 
        /// Has to have same orientation as value series
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
        public FormatColumn TimeFormat { get; set; }
    }
}
