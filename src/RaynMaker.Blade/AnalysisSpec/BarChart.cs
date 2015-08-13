using System.ComponentModel.DataAnnotations;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Documents;
using RaynMaker.Blade.Engine;
using RaynMaker.Blade.Entities;
using DatumSeries = RaynMaker.Blade.DataSheetSpec.Series;

namespace RaynMaker.Blade.AnalysisSpec
{
    public class BarChart : IReportElement
    {
        public string Caption { get; set; }

        [Required]
        public string Source { get; set; }

        public void Report( ReportContext context )
        {
            var series = ( IDatumSeries )context.ProvideValue( Source ) ?? DatumSeries.Empty;
            var caption = Caption ?? series.Name;

            var chart = new Chart();
            var barSeries = new BarSeries();
            barSeries.Title = caption;
            barSeries.ItemsSource = series;
            barSeries.IndependentValuePath = "Period";
            barSeries.DependentValuePath = "Value";

            chart.Series.Add( barSeries );

            context.Document.Blocks.Add( new BlockUIContainer( chart ) );
        }
    }
}
