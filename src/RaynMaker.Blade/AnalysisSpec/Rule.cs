using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec
{
    public class Rule : IReportElement
    {
        [Required]
        public string Caption { get; set; }

        [Required]
        public Operator Operator { get; set; }

        [Required]
        public double Threshold { get; set; }

        public Currency Currency { get; set; }

        [Required]
        public string Source { get; set; }

        [Required]
        public string Value { get; set; }

        public bool InMillions { get; set; }

        public void Report( ReportContext context )
        {
            var provider = context.GetProvider( Value );
            var value = ( IDatum )provider.ProvideValue( context.Asset );
            var valueCurrency = value is ICurrencyDatum ? ( ( ICurrencyDatum )value ).Currency : null;
            var threshold = Currency != null ? context.TranslateCurrency( Threshold, Currency, valueCurrency ) : Threshold;

            bool success = Operator.Compare( value.Value, threshold );

            var paragraph = new Paragraph()
            {
                Padding = new Thickness( 2 ),
                Background = success ? Brushes.LightGreen : Brushes.OrangeRed,
            };

            paragraph.Inlines.Add( new Run( "Rule: " ) { FontWeight = FontWeights.DemiBold } );

            paragraph.Inlines.Add( new Run(
                string.Format( "{0} {1} {2} {3} {4}{5} ({6:0.00}%)",
                Caption,
                FormatValue( value.Value ),
                success ? "" : " NOT",
                Operator.Name,
                FormatValue( threshold ),
                valueCurrency != null ? " " + valueCurrency.Name : "",
                ( value.Value - threshold ) / threshold * 100
                ) ) );

            context.Document.Blocks.Add( paragraph );
        }

        private string FormatValue( double value )
        {
            if( InMillions )
            {
                return string.Format( "{0:#,0.00} Mio", value / 1000 / 1000 );
            }
            else
            {
                //... .ToString("#,##0.00")
                return string.Format( "{0:#,0.00}", value );
            }
        }
    }
}
