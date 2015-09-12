using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using Plainion;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Extensions to System.Data.Table.
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary>
        /// Dumps the table content to Console.Out.
        /// </summary>
        public static void Dump( this DataTable table )
        {
            table.Rows.ToSet().Dump();
        }
        /// <summary>
        /// Dumps the table content as XML to Console.Out.
        /// </summary>
        public static void DumpAsXml( this DataTable table )
        {
            table.WriteXml( Console.Out );
        }
        /// <summary>
        /// Extracts a cell or a series of the DataTable the given "anchor" is pointing to.
        /// If the "anchor" is "empty" the whole input table is returned.
        /// The series to be extracted is always arranged in a column (independed of the original layout
        /// in the html table). The first column contains the values, the second the series header (if
        /// any defined). The series name is stored in the ColumnName of the first column.
        /// </summary>
        public static DataTable ExtractSeries( this DataTable table, Point anchor, Func<object, object> GetValue, TableExtractionSettings settings )
        {
            Contract.RequiresNotNull( table, "table" );

            if( anchor.X < 0 || anchor.Y < 0 )
            {
                throw new InvalidExpressionException( "Path expression corrupt: cell position in table could not be calculated" );
            }
            if( anchor.X >= table.Columns.Count || anchor.Y >= table.Rows.Count )
            {
                throw new InvalidExpressionException( "Path points to cell outside table" );
            }
            DataTable result = new DataTable();
            result.Locale = table.Locale;
            Action<bool> action = delegate( bool addHeaderCol )
            {
                result.Columns.Add( new DataColumn( "Values", settings.SeriesValueType ) );
                if( addHeaderCol )
                {
                    result.Columns.Add( new DataColumn( "Header", settings.SeriesHeaderType ) );
                }
            };
            Action<object, object> action2 = delegate( object value, object header )
            {
                DataRow dataRow2 = result.NewRow();
                result.Rows.Add( dataRow2 );
                dataRow2[ 0 ] = Convert.ChangeType( GetValue( value ), settings.SeriesValueType );
                if( header != null )
                {
                    dataRow2[ 1 ] = Convert.ChangeType( header, settings.SeriesHeaderType );
                }
            };
            Func<int, int, string> func = delegate( int row, int col )
            {
                if( row != -1 && col != -1 )
                {
                    return Convert.ToString( table.Rows[ row ][ col ], result.Locale );
                }
                return null;
            };
            if( settings.Dimension == CellDimension.Row )
            {
                action( settings.ColumnHeaderRow != -1 );
                DataRow dataRow = table.Rows[ anchor.Y ];
                string text = null;
                for( int i = 0; i < dataRow.ItemArray.Length; i++ )
                {
                    if( i == settings.RowHeaderColumn )
                    {
                        text = Convert.ToString( GetValue( dataRow[ i ] ), result.Locale );
                    }
                    else
                    {
                        if( !settings.SkipColumns.Contains( i ) )
                        {
                            object arg = dataRow[ i ];
                            string arg2 = func( settings.ColumnHeaderRow, i );
                            action2( arg, arg2 );
                        }
                    }
                }
                if( text != null )
                {
                    result.Columns[ 0 ].ColumnName = text;
                }
            }
            else
            {
                if( settings.Dimension == CellDimension.Column )
                {
                    action( settings.RowHeaderColumn != -1 );
                    string text2 = null;
                    for( int j = 0; j < table.Rows.Count; j++ )
                    {
                        if( j == settings.ColumnHeaderRow )
                        {
                            text2 = Convert.ToString( GetValue( table.Rows[ j ][ anchor.X ] ), result.Locale );
                        }
                        else
                        {
                            if( !settings.SkipRows.Contains( j ) )
                            {
                                object arg3 = table.Rows[ j ][ anchor.X ];
                                string arg4 = func( j, settings.RowHeaderColumn );
                                action2( arg3, arg4 );
                            }
                        }
                    }
                    if( text2 != null )
                    {
                        result.Columns[ 0 ].ColumnName = text2;
                    }
                }
                else
                {
                    object arg5 = table.Rows[ anchor.Y ][ anchor.X ];
                    string arg6 = func( anchor.Y, settings.RowHeaderColumn );
                    string text3 = func( settings.ColumnHeaderRow, anchor.X );
                    action( settings.RowHeaderColumn != -1 );
                    if( text3 != null )
                    {
                        result.Columns[ 0 ].ColumnName = text3;
                    }
                    action2( arg5, arg6 );
                }
            }
            result.AcceptChanges();
            if( settings.SeriesName != null && !result.Columns[ 0 ].ColumnName.Contains( settings.SeriesName ) )
            {
                throw new InvalidExpressionException( string.Format( "Validation of series name failed: '{0}' does not contain '{1}'", result.Columns[ 0 ].ColumnName, settings.SeriesName ) );
            }
            return result;
        }
        /// <summary>
        /// Writes the content of the table to the given writer in CSV
        /// format.
        /// The column description is not written.
        /// </summary>
        public static void WriteCsv( this DataTable table, TextWriter writer, string separator )
        {
            table.Rows.ToSet().WriteCsv( writer, separator );
        }
        /// <summary>
        /// Grows the table so that the given row and column index are inside.
        /// </summary>
        public static void GrowToCell( this DataTable table, int row, int column )
        {
            int rows = Math.Max( row + 1, table.Rows.Count );
            int columns = Math.Max( column + 1, table.Columns.Count );
            table.ResizeTable( rows, columns );
        }
        /// <summary>
        /// Resizes the table to the given number of rows and columns.
        /// </summary>
        public static void ResizeTable( this DataTable table, int rows, int columns )
        {
            while( columns > table.Columns.Count )
            {
                table.Columns.Add( new DataColumn() );
            }
            while( columns < table.Columns.Count )
            {
                table.Columns.RemoveAt( table.Columns.Count - 1 );
            }
            while( rows > table.Rows.Count )
            {
                table.Rows.Add( table.NewRow() );
            }
            while( rows < table.Rows.Count )
            {
                table.Rows.RemoveAt( table.Rows.Count - 1 );
            }
            table.AcceptChanges();
        }
        /// <summary>
        /// Remove rows and columns from the end which are completely empty.
        /// </summary>
        public static void FitToContent( this DataTable table )
        {
            int num = 0;
            int num2 = 0;
            for( int i = 0; i < table.Rows.Count; i++ )
            {
                DataRow row = table.Rows[ i ];
                if( !row.IsEmpty() )
                {
                    num2 = i;
                }
                num = Math.Max( num, DataTableExtensions.GetLastFilledColumn( row ) );
            }
            table.ResizeTable( num2 + 1, num + 1 );
        }
        private static int GetLastFilledColumn( DataRow row )
        {
            int result = 0;
            for( int i = 0; i < row.ItemArray.Length; i++ )
            {
                if( !DataTableExtensions.IsEmptyCell( ( DataTable )null, row[ i ] ) )
                {
                    result = i;
                }
            }
            return result;
        }
        /// <summary>
        /// Defines what "empty" for a cell in a DataTable means.
        /// </summary>
        /// <param name="cell">cell content</param>
        /// <param name="table">ignored - just for syntactic sugar</param>
        public static bool IsEmptyCell( this DataTable table, object cell )
        {
            return cell == null || DBNull.Value.Equals( cell );
        }
    }
}
