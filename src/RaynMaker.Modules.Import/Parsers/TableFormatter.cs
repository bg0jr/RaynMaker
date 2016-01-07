using System;
using System.Data;
using System.Linq;
using Plainion;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.Parsers
{
    class TableFormatter
    {
        public static DataTable ToFormattedTable( TableDescriptorBase descriptor, DataTable rawTable )
        {
            DataTable table = new DataTable();
            foreach ( var col in descriptor.Columns )
            {
                table.Columns.Add( col.Name, col.Type );
            }
            ToFormattedTable( descriptor, rawTable, table );

            return table;
        }

        public static void ToFormattedTable( TableDescriptorBase descriptor, DataTable rawTable, DataTable targetTable )
        {
            for ( int r = 0; r < rawTable.Rows.Count; ++r )
            {
                if ( descriptor.SkipRows.Contains( r ) )
                {
                    continue;
                }

                DataRow rawRow = rawTable.Rows[ r ];
                DataRow row = targetTable.NewRow();
                int targetCol = 0;
                bool isEmpty = true;
                for ( int c = 0; c < rawRow.ItemArray.Length; ++c )
                {
                    if ( descriptor.SkipColumns.Contains( c ) )
                    {
                        continue;
                    }

                    if ( targetCol == descriptor.Columns.Count )
                    {
                        break;
                    }

                    FormatColumn formatCol = descriptor.Columns[ targetCol ];
                    object value = formatCol.Convert( (string)rawRow[ c ] );
                    row[ formatCol.Name ] = ( value != null ? value : DBNull.Value );
                    if ( row[ formatCol.Name ] != DBNull.Value )
                    {
                        isEmpty = false;
                    }

                    targetCol++;
                }

                if ( !isEmpty )
                {
                    targetTable.Rows.Add( row );
                }
            }
        }

        public static DataTable ToFormattedTable( SeriesDescriptorBase descriptor, DataTable rawTable )
        {
            Contract.RequiresNotNull( descriptor, "descriptor" );
            Contract.RequiresNotNull( rawTable, "rawTable" );
            Contract.RequiresNotNull( descriptor.ValueFormat, "descriptor.ValueFormat" );
            Contract.Requires( descriptor.Orientation == SeriesOrientation.Row || descriptor.Orientation == SeriesOrientation.Column, "Unknown SeriesOrientation: " + descriptor.Orientation );

            var result = new DataTable();
            result.Locale = rawTable.Locale;

            result.Columns.Add( new DataColumn( descriptor.ValueFormat.Name, descriptor.ValueFormat.Type ) );
            if ( descriptor.TimesLocator != null && descriptor.TimeFormat != null )
            {
                result.Columns.Add( new DataColumn( descriptor.TimeFormat.Name, descriptor.TimeFormat.Type ) );
            }

            Action<object, object> AddValues = ( value, time ) =>
            {
                DataRow dataRow2 = result.NewRow();
                result.Rows.Add( dataRow2 );

                dataRow2[ 0 ] = descriptor.ValueFormat.Convert( (string)value );

                if ( time != null )
                {
                    dataRow2[ 1 ] = descriptor.TimeFormat.Convert( (string)time );
                }
            };

            Func<int, int, string> GetTimeValue = ( row, col ) =>
            {
                if ( row != -1 && col != -1 )
                {
                    return (string)rawTable.Rows[ row ][ col ];
                }
                return null;
            };

            if ( descriptor.Orientation == SeriesOrientation.Row )
            {
                int valuesColToScan = descriptor.ValuesLocator.HeaderSeriesPosition;
                Contract.Requires( valuesColToScan < rawTable.Columns.Count, "ValuesLocator points outside table" );

                var valuesRowIdx = descriptor.ValuesLocator.FindIndex( rawTable.Rows.ToSet().Select( row => row[ valuesColToScan ].ToString() ) );
                Contract.Invariant( valuesRowIdx != -1, "ValuesLocator condition failed: column not found" );

                var timesRowIdx = -1;
                if ( descriptor.TimesLocator != null && descriptor.TimeFormat != null )
                {
                    var timesColToScan = descriptor.TimesLocator.HeaderSeriesPosition;
                    Contract.Requires( timesColToScan < rawTable.Columns.Count, "TimesLocator points outside table" );

                    timesRowIdx = descriptor.TimesLocator.FindIndex( rawTable.Rows.ToSet().Select( row => row[ timesColToScan ].ToString() ) );
                    Contract.Invariant( timesRowIdx != -1, "TimesLocator condition failed: column not found" );
                }

                var dataRow = rawTable.Rows[ valuesRowIdx ];
                for ( int i = 0; i < dataRow.ItemArray.Length; i++ )
                {
                    if ( i != valuesColToScan && !descriptor.Excludes.Contains( i ) )
                    {
                        var value = dataRow[ i ];
                        var time = GetTimeValue( timesRowIdx, i );
                        AddValues( value, time );
                    }
                }
            }
            else //column
            {
                int valuesRowToScan = descriptor.ValuesLocator.HeaderSeriesPosition;
                Contract.Invariant( valuesRowToScan < rawTable.Rows.Count, "ValuesLocator points outside table" );

                var valuesColIdx = descriptor.ValuesLocator.FindIndex( rawTable.Rows[ valuesRowToScan ].ItemArray.Select( item => item.ToString() ) );
                Contract.Invariant( valuesColIdx != -1, "ValuesLocator condition failed: row not found" );

                var timesColIdx = -1;
                if ( descriptor.TimesLocator != null && descriptor.TimeFormat != null )
                {
                    var timesRowToScan = descriptor.TimesLocator.HeaderSeriesPosition;
                    Contract.Invariant( timesRowToScan < rawTable.Rows.Count, "TimesLocator points outside table" );

                    timesColIdx = descriptor.TimesLocator.FindIndex( rawTable.Rows[ timesRowToScan ].ItemArray.Select( item => item.ToString() ) );
                    Contract.Invariant( timesColIdx != -1, "TimesLocator condition failed: row not found" );
                }

                for ( int i = 0; i < rawTable.Rows.Count; i++ )
                {
                    if ( i != valuesRowToScan && !descriptor.Excludes.Contains( i ) )
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

        internal static object GetValue( PathCellDescriptor descriptor, DataTable inputTable )
        {
            Contract.RequiresNotNull( descriptor, "descriptor" );
            Contract.RequiresNotNull( inputTable, "inputTable" );

            int rowToScan = descriptor.Column.HeaderSeriesPosition;
            Contract.Requires( rowToScan < inputTable.Rows.Count, "ValuesLocator points outside table" );

            var colIdx = descriptor.Column.FindIndex( inputTable.Rows[ rowToScan ].ItemArray.Select( item => item.ToString() ) );
            Contract.Invariant( colIdx != -1, "ValuesLocator condition failed: column not found" );

            var colToScan = descriptor.Row.HeaderSeriesPosition;
            Contract.Requires( colToScan < inputTable.Columns.Count, "TimesLocator points outside table" );

            var rowIdx = descriptor.Row.FindIndex( inputTable.Rows.ToSet().Select( row => row[ colToScan ].ToString() ) );
            Contract.Invariant( colIdx != -1, "TimesLocator condition failed: column not found" );

            var value = inputTable.Rows[ rowIdx ][ colIdx ];

            return descriptor.ValueFormat.Convert( (string)value );
        }
    }
}
