using System;
using System.IO;
using RaynMaker.Blade.Sdk;

namespace RaynMaker.Blade
{
    class StockAnalyzer
    {
        private TextWriter myWriter;

        public StockAnalyzer( TextWriter writer )
        {
            myWriter = writer;
        }

        internal void Execute( Stock stock )
        {
            Console.WriteLine( "Analyzing: {0} - Isin: {1}", stock.Name, stock.Isin );
        }
    }
}
