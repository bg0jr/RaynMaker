using System;
using System.Diagnostics;
using System.IO;
using Plainion;
using Plainion.Logging;
using Plainion.Xaml;
using RaynMaker.Blade.AnalysisSpec;
using RaynMaker.Blade.DataSheetSpec;

namespace RaynMaker.Blade
{
    class Program
    {
        private static ILogger myLogger = LoggerFactory.GetLogger( typeof( Program ) );

        private static string myAnalysis;
        private static string myDataSheet;

        static void Main( string[] args )
        {
            try
            {
                ParseCommandLineArgs( args );

                var reader = new ValidatingXamlReader();
                var analysisTemplate = reader.Read<AnalysisTemplate>( myAnalysis );

                var sheet = reader.Read<DataSheet>( myDataSheet );

                if( sheet.Asset is Stock )
                {
                    var analyzer = new StockAnalyzer( analysisTemplate.Analysis, Console.Out );
                    analyzer.Execute( ( Stock )sheet.Asset );
                }
                else
                {
                    throw new NotSupportedException( "Asset type not supported: " + sheet.Asset.GetType() );
                }

                if( Debugger.IsAttached )
                {
                    Console.ReadLine();
                }
            }
            catch( Exception ex )
            {
                myLogger.Error( ex, "Failed to process input" );
            }
        }

        private static void ParseCommandLineArgs( string[] args )
        {
            for( int i = 0; i < args.Length; ++i )
            {
                if( args[ i ] == "-h" || args[ i ] == "--help" )
                {
                    Usage();
                    Environment.Exit( 0 );
                }
                else if( args[ i ] == "-debug")
                {
                    Debugger.Launch();
                }
                else if( args[ i ] == "-a" || args[ i ] == "--analysis" )
                {
                    Contract.Requires( i + 1 < args.Length, "-a requires an argument" );
                    i++;
                    myAnalysis = args[ i ];
                }
                else
                {
                    myDataSheet = args[ i ];
                }
            }

            Contract.Requires( myAnalysis != null, "No analysis template given" );
            Contract.Requires( myDataSheet != null, "No datasheet given" );
        }

        private static void Usage()
        {
            Console.WriteLine( "{0} [options] <datasheet>", Path.GetFileName( typeof( Program ).GetType().Assembly.Location ) );
            Console.WriteLine();
            Console.WriteLine( "Options:" );
            Console.WriteLine( "  -h/--help                 print usage" );
            Console.WriteLine( "  -a/--analysis <file>      path to analysis template" );
            Console.WriteLine();
        }
    }
}
