using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Markup;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec
{
    [DefaultProperty( "Text" ), ContentProperty( "Text" )]
    public class TextBlock : IReportElement
    {
        [Required]
        public string Text { get; set; }

        public void Report( ReportContext context )
        {
            context.Out.WriteLine( context.Evaluate( Text) );
        }
    }
}
