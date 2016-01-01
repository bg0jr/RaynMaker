using System;
using System.Data;
using System.Linq;
using Plainion;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.Parsers
{
    class TableFormatter
    {
        /// <summary>
        /// Tries to format the given DataTable as descripbed in the 
        /// FormatColumns. That means: types are converted into the 
        /// required types, only the described part of the raw table 
        /// is extracted.
        /// Empty rows will be removed.
        /// </summary>
        public static DataTable ToFormattedTable( TableDescriptorBase format, DataTable rawTable )
        {
            DataTable table = new DataTable();
            foreach( var col in format.Columns )
            {
                table.Columns.Add( col.Name, col.Type );
            }
            ToFormattedTable( format, rawTable, table );

            return table;
        }

        public static void ToFormattedTable( TableDescriptorBase format, DataTable rawTable, DataTable targetTable )
        {
            for( int r = 0; r < rawTable.Rows.Count; ++r )
            {
                if( format.SkipRows.Contains( r ) )
                {
                    continue;
                }

                DataRow rawRow = rawTable.Rows[ r ];
                DataRow row = targetTable.NewRow();
                int targetCol = 0;
                bool isEmpty = true;
                for( int c = 0; c < rawRow.ItemArray.Length; ++c )
                {
                    if( format.SkipColumns.Contains( c ) )
                    {
                        continue;
                    }

                    if( targetCol == format.Columns.Length )
                    {
                        break;
                    }

                    FormatColumn formatCol = format.Columns[ targetCol ];
                    object value = formatCol.Convert( rawRow[ c ].ToString() );
                    row[ formatCol.Name ] = ( value != null ? value : DBNull.Value );
                    if( row[ formatCol.Name ] != DBNull.Value )
                    {
                        isEmpty = false;
                    }

                    targetCol++;
                }

                if( !isEmpty )
                {
                    targetTable.Rows.Add( row );
                }
            }
        }

        /// <summary>
        /// Tries to format the given DataTable as descripbed in 
        /// ValueFormat and TimeAxisFormat. That means: types are 
        /// converted into the required types, the first column
        /// is treated as value column the second as timeaxis column.
        /// <remarks>Usually this method is called with a table
        /// which has been tailored using DataTable.ExtractSeries()
        /// </remarks>
        /// </summary>
        public static DataTable ToFormattedTable( SeriesDescriptorBase format, DataTable table_in )
        {
            if( table_in == null )
            {
                throw new ArgumentNullException( "table_in" );
            }

            DataTable rawTable = table_in;

            rawTable = ExtractSeries( format, table_in );
            if( rawTable == null )
            {
                return null;
            }

            if( format.ValueFormat == null && format.TimeFormat == null )
            {
                return rawTable;
            }

            DataTable table = new DataTable();

            if( format.ValueFormat != null )
            {
                table.Columns.Add( new DataColumn( format.ValueFormat.Name, format.ValueFormat.Type ) );
            }
            else
            {
                throw new InvalidOperationException( "No value format specified" );
            }

            if( format.TimeFormat != null )
            {
                table.Columns.Add( new DataColumn( format.TimeFormat.Name, format.TimeFormat.Type ) );
            }

            foreach( DataRow rawRow in rawTable.Rows )
            {
                DataRow row = table.NewRow();

                Action<int, FormatColumn> Convert = ( idx, col ) =>
                {
                    object value = ( col == null ? rawRow[ idx ] : col.Convert( rawRow[ idx ].ToString() ) );
                    row[ idx ] = ( value != null ? value : DBNull.Value );
                };

                Convert( 0, format.ValueFormat );
                if( format.TimeFormat != null )
                {
                    Convert( 1, format.TimeFormat );
                }

                table.Rows.Add( row );
            }

            return table;
        }

        private static DataTable ExtractSeries( SeriesDescriptorBase format, DataTable rawTable )
        {
            Contract.Requires( format.Orientation == SeriesOrientation.Row || format.Orientation == SeriesOrientation.Column, "Unknown SeriesOrientation: " + format.Orientation );

            var result = new DataTable();
            result.Locale = rawTable.Locale;

            result.Columns.Add( new DataColumn( format.ValueFormat.Name, format.ValueFormat.Type ) );
            if( format.TimesLocator != null && format.TimeFormat != null )
            {
                result.Columns.Add( new DataColumn( format.TimeFormat.Name, format.TimeFormat.Type ) );
            }

            Action<object, object> AddValues = ( value, time ) =>
            {
                DataRow dataRow2 = result.NewRow();
                result.Rows.Add( dataRow2 );

                dataRow2[ 0 ] = Convert.ChangeType( value, format.ValueFormat.Type );

                if( time != null )
                {
                    dataRow2[ 1 ] = Convert.ChangeType( time, format.TimeFormat.Type );
                }
            };

            Func<int, int, string> GetTimeValue = ( row, col ) =>
            {
                if( row != -1 && col != -1 )
                {
                    return Convert.ToString( rawTable.Rows[ row ][ col ], result.Locale );
                }
                return null;
            };

            if( format.Orientation == SeriesOrientation.Row )
            {
                int valuesColToScan = format.ValuesLocator.HeaderSeriesPosition;
                Contract.Requires( valuesColToScan < rawTable.Columns.Count, "ValuesLocator points outside table" );

                var valuesRowIdx = format.ValuesLocator.FindIndex( rawTable.Rows.ToSet().Select( row => row[ valuesColToScan ].ToString() ) );
                Contract.Invariant( valuesRowIdx != -1, "ValuesLocator condition failed: column not found" );

                var timesRowIdx = -1;
                if( format.TimesLocator != null && format.TimeFormat != null )
                {
                    var timesColToScan = format.TimesLocator.HeaderSeriesPosition;
                    Contract.Requires( timesColToScan < rawTable.Columns.Count, "TimesLocator points outside table" );

                    timesRowIdx = format.TimesLocator.FindIndex( rawTable.Rows.ToSet().Select( row => row[ timesColToScan ].ToString() ) );
                    Contract.Invariant( timesRowIdx != -1, "TimesLocator condition failed: column not found" );
                }

                var dataRow = rawTable.Rows[ valuesRowIdx ];
                for( int i = 0; i < dataRow.ItemArray.Length; i++ )
                {
                    if( i != valuesColToScan && !format.Excludes.Contains( i ) )
                    {
                        var value = dataRow[ i ];
                        var time = GetTimeValue( timesRowIdx, i );
                        AddValues( value, time );
                    }
                }
            }
            else //column
            {
                int valuesRowToScan = format.ValuesLocator.HeaderSeriesPosition;
                Contract.Invariant( valuesRowToScan < rawTable.Rows.Count, "ValuesLocator points outside table" );

                var valuesColIdx = format.ValuesLocator.FindIndex( rawTable.Rows[ valuesRowToScan ].ItemArray.Select( item => item.ToString() ) );
                Contract.Invariant( valuesColIdx != -1, "ValuesLocator condition failed: row not found" );

                var timesColIdx = -1;
                if( format.TimesLocator != null && format.TimeFormat != null )
                {
                    var timesRowToScan = format.TimesLocator.HeaderSeriesPosition;
                    Contract.Invariant( timesRowToScan < rawTable.Rows.Count, "TimesLocator points outside table" );

                    timesColIdx = format.TimesLocator.FindIndex( rawTable.Rows[ timesRowToScan ].ItemArray.Select( item => item.ToString() ) );
                    Contract.Invariant( timesColIdx != -1, "TimesLocator condition failed: row not found" );
                }

                for( int i = 0; i < rawTable.Rows.Count; i++ )
                {
                    if( i != valuesRowToScan && !format.Excludes.Contains( i ) )
                    {
                        var value = rawTable.Rows[ i ][ valuesColIdx ];
                        var time = GetTimeValue( i, timesColIdx );
                        AddValues( value, time );
                    }
                }
            }

            result.AcceptChanges();

            return result;
        }
    }
}
