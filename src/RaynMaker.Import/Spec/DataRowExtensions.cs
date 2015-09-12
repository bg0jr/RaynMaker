using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Extension to the System.Data.DataRow class.
    /// </summary>
    public static class DataRowExtensions
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
        /// Returns the sorted set of all indices of the given list.
        /// </summary>
        public static IEnumerable<int> Indices( this DataRowCollection list )
        {
            for( int i = 0; i < list.Count; i++ )
            {
                yield return i;
            }
            yield break;
        }
        /// <summary>
        /// Dumps the table content to Console.Out.
        /// </summary>
        public static void WriteCsv( this IEnumerable<DataRow> rows, TextWriter writer, string separator )
        {
            if( rows.FirstOrDefault<DataRow>() == null )
            {
                return;
            }
            foreach( DataRow current in rows )
            {
                for( int i = 0; i < current.ItemArray.Length; i++ )
                {
                    writer.Write( current.ItemArray[ i ] );
                    if( i + 1 < current.ItemArray.Length )
                    {
                        writer.Write( separator );
                    }
                }
                writer.WriteLine();
            }
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
        /// <summary>
        /// Returns true if the complete row is empty.
        /// </summary>
        public static bool IsEmpty( this DataRow row )
        {
            return row.ItemArray.All( ( object cell ) => DataTableExtensions.IsEmptyCell( ( DataTable )null, cell ) );
        }
    }
}
