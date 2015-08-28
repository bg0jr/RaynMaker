using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaynMaker.Import.Spec
{
    public class AbsolutePositionLocator : ICellLocator
    {
        public AbsolutePositionLocator( int position )
        {
            Position = position;
        }

        /// <summary>
        /// Describes the absolute row/column position depending on the dimension.
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Returns always zero because this locator is independent of
        /// the series to scan.
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
