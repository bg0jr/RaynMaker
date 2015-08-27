using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec
{
    [DefaultProperty( "Text" ), ContentProperty( "Text" )]
    public class TextBlock : IReportElement
    {
        [Required]
        public string Text { get; set; }

        public string Caption { get; set; }

        public void Report( ReportContext context )
        {
            if( Caption != null )
            {
                var paragraph = new Paragraph();
                paragraph.Inlines.Add( new Run( Caption ) { FontWeight = FontWeights.DemiBold } );
                paragraph.Inlines.Add( new Run( context.Evaluate( Text ) ) );
                context.Document.Blocks.Add( paragraph );
            }
            else
            {
                context.Document.Paragraph( context.Evaluate( Text ) );
            }
        }
    }
}
