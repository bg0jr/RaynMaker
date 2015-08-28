using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Controls.DataVisualization;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using RaynMaker.Analysis.Engine;
using RaynMaker.Entities;

namespace RaynMaker.Analysis.AnalysisSpec
{
    public class BarChart : IReportElement
    {
        public string Caption { get; set; }

        public bool InMillions { get; set; }

        [Required]
        public string Source { get; set; }

        public void Report( ReportContext context )
        {
            var series = ( IDatumSeries )context.ProvideValue( Source ) ?? DatumSeries.Empty;
            var caption = Caption ?? series.Name;

            if( InMillions )
            {
                caption += " (in Mio.)";
            }

            var chart = new Chart();
            chart.Title = caption;
            chart.Width = 200;
            chart.Height = 200;
            chart.BorderThickness = new Thickness( 0 );

            var style = new Style( typeof( Legend ) );
            style.Setters.Add( new Setter( FrameworkElement.WidthProperty, 0d ) );
            style.Setters.Add( new Setter( FrameworkElement.HeightProperty, 0d ) );
            chart.LegendStyle = style;

            style = new Style( typeof( Title ) );
            style.Setters.Add( new Setter( Title.FontSizeProperty, 14d ) );
            style.Setters.Add( new Setter( Title.HorizontalAlignmentProperty, HorizontalAlignment.Center ) );
            chart.TitleStyle = style;

            var chartSeries = new ColumnSeries();
            chartSeries.ItemsSource = series
                .OrderBy( i => i.Period )
                .ToList();

            chartSeries.IndependentValueBinding = new Binding( "Period" )
                {
                    Converter = new PeriodChartConverter()
                };
            chartSeries.DependentValueBinding = new Binding( "Value" )
                {
                    Converter = new InMillionsConverter() { InMillions = InMillions }
                };

            chart.Series.Add( chartSeries );

            context.Document.Blocks.Add( new BlockUIContainer( chart ) );
        }
    }
}
