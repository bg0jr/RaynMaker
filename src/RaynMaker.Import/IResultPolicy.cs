using System.Collections.Generic;
using System.Data;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import
{
    /// <summary>
    /// Handles the "provided"/"fetched" result.
    /// Validates it and defines how to go on if invalid or incomplete.
    /// </summary>
    public interface IResultPolicy
    {
        /// <summary>
        /// Validates the given fetch result from the given site.
        /// Returns true if validation was successful and the fetch process
        /// can be aborted.
        /// </summary>
        bool Validate( Site site, DataTable result );

        /// <summary>
        /// The final result of the fetch process.
        /// </summary>
        DataTable ResultTable { get; }

        /// <summary>
        /// The sites which have produced the final result.
        /// </summary>
        IEnumerable<Site> Sites { get; }
    }
}
