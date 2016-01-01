using System;
using System.Data;
using System.Drawing;
using System.Linq;
using Plainion;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.Parsers
{
    /// <summary>
    /// Extensions to System.Data.Table.
    /// </summary>
    static class DataTableExtensions
    {
        /// <summary>
        /// Dumps the table content to Console.Out.
        /// </summary>
        public static void Dump( this DataTable table )
        {
            table.Rows.ToSet().Dump();
        }
    }
}
