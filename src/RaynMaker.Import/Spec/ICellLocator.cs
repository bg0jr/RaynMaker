using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Describes a position of a row or column of a data table.
    /// </summary>
    public interface ICellLocator
    {
        /// <summary>
        /// Returns the index of the series to scan for the requested
        /// index of the other dimension.
        /// </summary>
        int SeriesToScan { get; }

        /// <summary>
        /// Scans the given series and returns the requested index.
        /// <remarks>
        /// Get an IEnumerable of string here because the values we want to check are 
        /// always cells - never a DataColumn.ColumnName.
        /// </remarks>
        /// </summary>
        int GetLocation( IEnumerable<string> list );
    }
}
