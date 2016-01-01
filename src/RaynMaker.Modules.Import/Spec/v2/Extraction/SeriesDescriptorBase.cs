using System.Linq;
using System.Runtime.Serialization;

namespace RaynMaker.Modules.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Defines the properties common to most series describing descriptors
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "SeriesDescriptorBase" )]
    public abstract class SeriesDescriptorBase : FigureDescriptorBase
    {
        private int[] myExcludes = null;

        protected SeriesDescriptorBase( string name )
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
        public ISeriesLocator ValuesLocator { get; set; }

        [DataMember]
        public FormatColumn ValueFormat { get; set; }

        /// <summary>
        /// Describes the position of the times within a table.
        /// Orientation of the times series is described by the Orientation property.
        /// </summary>
        [DataMember]
        public ISeriesLocator TimesLocator { get; set; }

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
