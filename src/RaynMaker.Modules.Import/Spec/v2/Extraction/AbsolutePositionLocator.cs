using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Plainion;

namespace RaynMaker.Modules.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Locates a series by an absolute position explicitly given to constructor.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "AbsolutePositionLocator" )]
    public class AbsolutePositionLocator : ISeriesLocator
    {
        /// <summary>
        /// The HeaderSeriesPosition has still to be specified even if the series position is already clear because the HeaderSeriesPosition might
        /// be used later on to extract the header of the series.
        /// </summary>
        public AbsolutePositionLocator( int headerSeriesPosition, int seriesPosition )
        {
            Contract.Requires( headerSeriesPosition >= 0, "HeaderSeriesPosition must be greater or equal to 0" );
            Contract.Requires( seriesPosition >= 0, "SeriesPosition must be greater or equal to 0" );

            HeaderSeriesPosition = headerSeriesPosition;
            SeriesPosition = seriesPosition;
        }

        [DataMember]
        public int HeaderSeriesPosition { get; private set; }

        [DataMember]
        public int SeriesPosition { get; private set; }

        public int FindIndex( IEnumerable<string> items )
        {
            return SeriesPosition;
        }
    }
}
