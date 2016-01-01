﻿using System;
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

            var result = new DataTable();
            result.Locale = table.Locale;

            Action<bool> AddColumns = delegate( bool addHeaderCol )
            {
                result.Columns.Add( new DataColumn( "Values", settings.SeriesValueType ) );
                if( addHeaderCol )
                {
                    result.Columns.Add( new DataColumn( "Header", settings.SeriesHeaderType ) );
                }
            };

            Action<object, object> AddValues = delegate( object value, object header )
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

            if( settings.Dimension == SeriesOrientation.Row )
            {
                AddColumns( settings.ColumnHeaderRow != -1 );

                var dataRow = table.Rows[ anchor.Y ];
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
                            object value = dataRow[ i ];
                            string header = func( settings.ColumnHeaderRow, i );
                            AddValues( value, header );
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
                if( settings.Dimension == SeriesOrientation.Column )
                {
                    AddColumns( settings.RowHeaderColumn != -1 );

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
                                object value = table.Rows[ j ][ anchor.X ];
                                string header = func( j, settings.RowHeaderColumn );
                                AddValues( value, header );
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
                    AddColumns( settings.RowHeaderColumn != -1 );
                    if( text3 != null )
                    {
                        result.Columns[ 0 ].ColumnName = text3;
                    }
                    AddValues( arg5, arg6 );
                }
            }

            result.AcceptChanges();

            if( settings.SeriesName != null && !result.Columns[ 0 ].ColumnName.Contains( settings.SeriesName ) )
            {
                throw new InvalidExpressionException( string.Format( "Validation of series name failed: '{0}' does not contain '{1}'", result.Columns[ 0 ].ColumnName, settings.SeriesName ) );
            }

            return result;
        }
    }
}