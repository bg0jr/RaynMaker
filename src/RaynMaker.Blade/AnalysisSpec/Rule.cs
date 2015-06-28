using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.Engine;
using RaynMaker.Blade.Reporting;

namespace RaynMaker.Blade.AnalysisSpec
{
    public class Rule : IReportElement
    {
        [Required]
        public string Caption { get; set; }

        [Required]
        public string Operator { get; set; }

        [Required]
        public double Threshold { get; set; }

        public Currency Currency { get; set; }

        [Required]
        public string Source { get; set; }

        [Required]
        public string Value { get; set; }

        public void Report( ReportContext context )
        {
            // Rule: MarketCap GreaterThen XYZ Dollar - FAILED by ABC %
            // Rule: MarketCap ABC NOT GreaterThen XYZ Dollar (-5%)
            var paragraph = new Paragraph();

            paragraph.Inlines.Add( new Run( "Rule: " ) { FontWeight = FontWeights.DemiBold } );
            paragraph.Inlines.Add( new Run( Caption ) );
            paragraph.Inlines.Add( new Run( " " + Operator ) );
            paragraph.Inlines.Add( new Run( " " + Threshold ) );
            paragraph.Inlines.Add( new Run( " " + Currency.Name ) );

            context.Document.Blocks.Add( paragraph );
        }
    }
}
