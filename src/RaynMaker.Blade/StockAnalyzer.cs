using System;
using System.IO;
using RaynMaker.Blade.AnalysisSpec;
using RaynMaker.Blade.DataSheetSpec;

namespace RaynMaker.Blade
{
    class StockAnalyzer
    {
        private Analysis myAnalysis;
        private TextWriter myWriter;

        public StockAnalyzer( Analysis analysis, TextWriter writer )
        {
            myAnalysis = analysis;
            myWriter = writer;
        }

        internal void Execute( Stock stock )
        {
            Console.WriteLine( "Analyzing: {0} - Isin: {1}", stock.Name, stock.Isin );
        }
    }
}
