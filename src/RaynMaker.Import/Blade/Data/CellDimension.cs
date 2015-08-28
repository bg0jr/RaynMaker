using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blade.Data
{
    /// <summary>
    /// Defines the part of a table under inspection.
    /// </summary>
    public enum CellDimension
    {
        /// <summary>
        /// No dimension. The single cell.
        /// </summary>
        None,
        /// <summary>
        /// Complete row.
        /// </summary>
        Row,
        /// <summary>
        /// Complete column.
        /// </summary>
        Column
    }
}
