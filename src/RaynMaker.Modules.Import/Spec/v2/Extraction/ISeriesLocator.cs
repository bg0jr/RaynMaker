using System.Collections.Generic;

namespace RaynMaker.Modules.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Describes a position of a row or column of a data table.
    /// </summary>
    public interface ISeriesLocator
    {
        /// <summary>
        /// Returns the index of the series to scan.
        /// </summary>
        int SeriesToScan { get; }

        /// <summary>
        /// Scans the given series and returns the requested index.
        /// </summary>
        /// <returns>
        /// Returns the index of the found series or -1 if nothing could be found.
        /// </returns>
        int FindIndex( IEnumerable<string> items );
    }
}
