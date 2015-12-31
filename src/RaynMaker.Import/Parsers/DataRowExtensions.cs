using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace RaynMaker.Import.Parsers
{
    /// <summary>
    /// Extension to the System.Data.DataRow class.
    /// </summary>
    static class DataRowExtensions
    {
        /// <summary>
        /// Returns an IEnumerableT of the given DataRowCollection.
        /// </summary>
        public static IEnumerable<DataRow> ToSet( this DataRowCollection list )
        {
            foreach( DataRow dataRow in list )
            {
                yield return dataRow;
            }
            yield break;
        }

        /// <summary>
        /// Dumps the table content to Console.Out.
        /// </summary>
        public static void Dump( this IEnumerable<DataRow> rows )
        {
            DataRow dataRow = rows.FirstOrDefault<DataRow>();
            if( dataRow == null )
            {
                Console.WriteLine( "nothing to dump" );
                return;
            }
            DataTable table = dataRow.Table;
            if( table != null )
            {
                Console.WriteLine( "Dump of table: " + table.TableName );
                foreach( DataColumn dataColumn in table.Columns )
                {
                    Console.Write( string.Format( "{0," + DataRowExtensions.GetColumnWidth( dataColumn ) + "}", dataColumn.ColumnName ) );
                }
                Console.WriteLine();
            }
            foreach( DataRow current in rows )
            {
                for( int i = 0; i < current.ItemArray.Length; i++ )
                {
                    int columnWidth = DataRowExtensions.GetColumnWidth( ( table != null ) ? table.Columns[ i ] : null );
                    Console.Write( string.Format( "{0," + columnWidth + "}", current.ItemArray[ i ] ) );
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private static int GetColumnWidth( DataColumn col )
        {
            if( col == null )
            {
                return 12;
            }
            int num = col.ColumnName.Length + 1;
            if( num >= 12 )
            {
                return num;
            }
            return 12;
        }
    }
}
