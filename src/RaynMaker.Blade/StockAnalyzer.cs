using System.Linq;
using System.Windows.Documents;
using System.Windows.Media;
using Plainion;
using RaynMaker.Blade.AnalysisSpec;
using RaynMaker.Blade.Engine;
using RaynMaker.Blade.Entities;
using RaynMaker.Blade.Model;
using RaynMaker.Blade.Reporting;

namespace RaynMaker.Blade
{
    class StockAnalyzer
    {
        private Project myProject;
        private Analysis myAnalysis;

        public StockAnalyzer( Project project, Analysis analysis )
        {
            myProject = project;
            myAnalysis = analysis;
        }

        public void Execute( DataSheet sheet )
        {
            Contract.RequiresNotNull( sheet, "sheet" );

            var doc = new FlowDocument();
            doc.FontFamily = new FontFamily( "Arial" );
            doc.FontSize = 13;

            var stock = sheet.Company.Stocks.Single();

            doc.Headline( "{0} (Isin: {1})", stock.Company.Name, stock.Isin );

            var context = new ReportContext( myProject, stock, sheet, doc );
            foreach( var element in myAnalysis.Elements )
            {
                element.Report( context );
            }
            context.Complete();

            var report = new ReportView();
            report.Document = doc;
            report.Show();
        }
    }
}
