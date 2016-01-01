using System.Collections.Generic;

namespace RaynMaker.Modules.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Describes the position of a series within a table.
    /// The series can be a column or a row.
    /// </summary>
    public interface ISeriesLocator
    {
        /// <summary>
        /// Gets the index of the series containing the header of the described series.
        /// </summary>
        int HeaderSeriesPosition { get; }

        /// <summary>
        /// Scans the given series items (specified via <see cref="HeaderSeriesPosition"/>) and returns the index of the described series.
        /// </summary>
        /// <returns>
        /// Returns the index of the found series or -1 if nothing could be found.
        /// </returns>
        int FindIndex( IEnumerable<string> items );
    }
}
