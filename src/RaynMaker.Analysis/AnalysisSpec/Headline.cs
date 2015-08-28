using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Markup;
using RaynMaker.Analysis.Engine;

namespace RaynMaker.Analysis.AnalysisSpec
{
    [DefaultProperty( "Text" ), ContentProperty( "Text" )]
    public class Headline : IReportElement
    {
        [Required]
        public string Text { get; set; }

        public void Report( ReportContext context )
        {
            context.Document.Headline( context.Evaluate( Text ) );
        }
    }
}
