﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec.v2.Extraction
{
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "AbsolutePositionLocator" )]
    public class AbsolutePositionLocator : ISeriesLocator
    {
        public AbsolutePositionLocator( int position )
        {
            Position = position;
        }

        /// <summary>
        /// Describes the absolute row/column position depending on the dimension.
        /// </summary>
        [DataMember]
        public int Position { get; private set; }

        /// <summary>
        /// Returns always zero because this locator is independent of the series to scan.
        /// </summary>
        public int SeriesToScan
        {
            get { return 0; }
        }

        public int GetLocation( IEnumerable<string> list )
        {
            return Position;
        }
    }
}
