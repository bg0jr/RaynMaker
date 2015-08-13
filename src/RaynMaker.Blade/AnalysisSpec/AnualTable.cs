using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using Plainion;
using Plainion.Validation;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.Engine;
using RaynMaker.Blade.Entities;
using RaynMaker.Blade.Reporting;

namespace RaynMaker.Blade.AnalysisSpec
{
    [DefaultProperty( "Rows" ), ContentProperty( "Rows" )]
    public class AnualTable : IReportElement
    {
        public AnualTable()
        {
            Rows = new List<Row>();
        }

        [ValidateObject]
        public List<Row> Rows { get; private set; }

        [Required]
        public int EndYear { get; set; }

        [Required]
        public int Count { get; set; }

        public void Report( ReportContext context )
        {
            var table = new Table();
            table.CellSpacing = 5;

            table.Columns.Add( new TableColumn { Width = new GridLength( 200 ), Background = Brushes.AliceBlue } );
            Count.Times( i => table.Columns.Add( new TableColumn { Width = GridLength.Auto } ) );

            var rowGroup = new TableRowGroup();
            table.RowGroups.Add( rowGroup );

            var row = new TableRow { Background = Brushes.AliceBlue };
            row.Cells.Add( new TableCell() );

            for( int year = EndYear - Count + 1; year <= EndYear; ++year )
            {
                row.Cell( "{0,8}", year ).TextAlignment = TextAlignment.Right;
            }

            rowGroup.Rows.Add( row );

            foreach( var dataRow in Rows )
            {
                row = new TableRow();

                var series = ( IDatumSeries )context.ProvideValue( dataRow.Value ) ?? Series.Empty;

                var cell = row.Cell( GetHeader( dataRow, series ) );
                cell.TextAlignment = TextAlignment.Left;

                for( int year = EndYear - Count + 1; year <= EndYear; ++year )
                {
                    var period = new YearPeriod( year );
                    var value = series.SingleOrDefault( v => v.Period.Equals( period ) );
                    if( value == null )
                    {
                        row.Cell( "n.a." ).TextAlignment = TextAlignment.Right;
                    }
                    else if( dataRow.Round )
                    {
                        row.Cell( "{0:#,0}", value.Value ).TextAlignment = TextAlignment.Right;
                    }
                    else if( dataRow.InMillions )
                    {
                        row.Cell( "{0:#,0.00}", value.Value / 1000 / 1000 ).TextAlignment = TextAlignment.Right;
                    }
                    else
                    {
                        row.Cell( "{0:#,0.00}", value.Value ).TextAlignment = TextAlignment.Right;
                    }
                }

                rowGroup.Rows.Add( row );
            }

            context.Document.Blocks.Add( table );
        }

        private static string GetHeader( Row dataRow, IDatumSeries series )
        {
            if( series.Any() )
            {
                if( series.Currency == null )
                {
                    return dataRow.InMillions ? string.Format( "{0} (in Mio.)", dataRow.Caption ) : dataRow.Caption;
                }
                else
                {
                    if( dataRow.InMillions )
                    {
                        return string.Format( "{0} (in Mio. {1})", dataRow.Caption, series.Currency.Name );
                    }
                    else
                    {
                        return string.Format( "{0} ({1})", dataRow.Caption, series.Currency.Name );
                    }
                }
            }
            else
            {
                return dataRow.InMillions ? string.Format( "{0} (in Mio.)", dataRow.Caption ) : dataRow.Caption;
            }
        }
    }
}
