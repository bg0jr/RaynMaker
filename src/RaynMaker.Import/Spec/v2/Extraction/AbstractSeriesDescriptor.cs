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
    public abstract class AbstractSeriesDescriptor : AbstractFigureDescriptor
    {
        private int[] myExcludes = null;

        protected AbstractSeriesDescriptor( string name )
            : base( name )
        {
            Excludes = null;
        }

        [DataMember]
        public SeriesOrientation Orientation { get; set; }

        /// <summary>
        /// Describes the position of the values within a table.
        /// Orientation of the values series is described by the Orientation property.
        /// </summary>
        [DataMember]
        public ITablePositionLocator ValuesLocator { get; set; }

        [DataMember]
        public FormatColumn ValueFormat { get; set; }

        /// <summary>
        /// Describes the position of the times within a table.
        /// Orientation of the times series is described by the Orientation property.
        /// </summary>
        [DataMember]
        public ITablePositionLocator TimesLocator { get; set; }

        [DataMember]
        public FormatColumn TimeFormat { get; set; }

        [DataMember]
        public int[] Excludes
        {
            get { return myExcludes; }
            set { myExcludes = value == null ? new int[] { } : value.ToArray(); }
        }
    }
}
