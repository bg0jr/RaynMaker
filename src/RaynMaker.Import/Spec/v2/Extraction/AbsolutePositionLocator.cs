using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Locates a series by an absolute position explicitly given to constructor.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "AbsolutePositionLocator" )]
    public class AbsolutePositionLocator : ISeriesLocator
    {
        public AbsolutePositionLocator( int position )
        {
            Position = position;
        }

        [DataMember]
        public int Position { get; private set; }

        /// <summary>
        /// Always returns -1 because this locator is independent of the series to scan.
        /// </summary>
        public int SeriesToScan
        {
            get { return -1; }
        }

        public int FindIndex( IEnumerable<string> items )
        {
            return Position;
        }
    }
}
