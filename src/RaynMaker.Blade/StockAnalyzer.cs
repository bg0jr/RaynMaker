using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using RaynMaker.Blade.AnalysisSpec;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.Engine;
using RaynMaker.Blade.Reporting;

namespace RaynMaker.Blade
{
    class StockAnalyzer
    {
        private Analysis myAnalysis;

        public StockAnalyzer( Analysis analysis )
        {
            myAnalysis = analysis;
        }

        public void Execute( Stock stock )
        {
            var doc = new FlowDocument();
            doc.FontFamily = new FontFamily( "Arial" );
            doc.FontSize = 13;

            doc.Headline( "{0} (Isin: {1})", stock.Name, stock.Isin );

            var context = new ReportContext( stock, doc );
            foreach( var element in myAnalysis.Elements )
            {
                element.Report( context );
            }

            var report = new ReportView();
            report.Document = doc;
            report.Show();
        }
    }
}
