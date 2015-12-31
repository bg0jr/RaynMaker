using System.Collections.Generic;

namespace RaynMaker.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Describes a position of a row or column of a data table.
    /// </summary>
    public interface ISeriesLocator
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
