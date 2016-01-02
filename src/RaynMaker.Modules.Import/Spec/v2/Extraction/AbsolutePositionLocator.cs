using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Plainion;
using Plainion.Serialization;

namespace RaynMaker.Modules.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Locates a series by an absolute position explicitly given to constructor.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "AbsolutePositionLocator" )]
    public class AbsolutePositionLocator : SerializableBindableBase, ISeriesLocator
    {
        private int myHeaderSeriesPosition;
        private int mySeriesPosition;

        /// <summary>
        /// The HeaderSeriesPosition has still to be specified even if the series position is already clear because the HeaderSeriesPosition might
        /// be used later on to extract the header of the series.
        /// </summary>
        [Range( 0, int.MaxValue )]
        [DataMember]
        public int HeaderSeriesPosition
        {
            get { return myHeaderSeriesPosition; }
            set { SetProperty( ref myHeaderSeriesPosition, value ); }
        }

        [Range( 0, int.MaxValue )]
        [DataMember]
        public int SeriesPosition
        {
            get { return mySeriesPosition; }
            set { SetProperty( ref mySeriesPosition, value ); }
        }

        public int FindIndex( IEnumerable<string> items )
        {
            return SeriesPosition;
        }
    }
}
