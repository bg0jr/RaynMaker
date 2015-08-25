using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using RaynMaker.Blade.Engine;
using RaynMaker.Entities;

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

        [TypeConverter( typeof( CurrencyConverter ) )]
        public Currency Currency { get; set; }

        [Required]
        public string Source { get; set; }

        [Required]
        public string Value { get; set; }

        public bool InMillions { get; set; }

        public void Report( ReportContext context )
        {
            var value = ( IDatum )context.ProvideValue( Value );

            if( value == null )
            {
                ReportMissingData( context );
            }
            else
            {
                ExecuteRule( context, value );
            }
        }

        private void ReportMissingData( ReportContext context )
        {
            var paragraph = new Paragraph()
            {
                Padding = new Thickness( 2 ),
                Background = Brushes.Red,
            };

            ReportRuleLabel( paragraph );
            paragraph.Inlines.Add( new Run( " FAILED: no value found") );

            context.Document.Blocks.Add( paragraph );
        }

        private void ExecuteRule( ReportContext context, IDatum value )
        {
            var valueCurrency = value is ICurrencyDatum ? ( ( ICurrencyDatum )value ).Currency : null;
            var threshold = Currency != null ? context.TranslateCurrency( Threshold, Currency, valueCurrency ) : Threshold;

            bool success = Operator.Compare( value.Value.Value, threshold );

            var paragraph = new Paragraph()
            {
                Padding = new Thickness( 2 ),
                Background = success ? Brushes.LightGreen : Brushes.OrangeRed,
            };

            ReportRuleLabel( paragraph );

            paragraph.Inlines.Add( new Run(
                string.Format( "{0} {1} {2}{3} ({4:0.00}%)",
                FormatValue( value.Value.Value ),
                Operator.Name,
                FormatValue( threshold ),
                valueCurrency != null ? " " + valueCurrency.Name : "",
                ( value.Value - threshold ) / threshold * 100
                ) ) );

            context.Document.Blocks.Add( paragraph );
        }

        private void ReportRuleLabel( Paragraph paragraph )
        {
            paragraph.Inlines.Add( new Run(
                string.Format( "{0}: ", Caption ) )
                {
                    FontWeight = FontWeights.DemiBold
                } );
        }

        private string FormatValue( double value )
        {
            if( InMillions )
            {
                return string.Format( "{0:#,0.00} Mio", value / 1000 / 1000 );
            }
            else
            {
                return string.Format( "{0:#,0.00}", value );
            }
        }
    }
}
