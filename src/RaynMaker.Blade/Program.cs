using System;
using System.IO;
using Plainion;
using Plainion.Logging;
using Plainion.Xaml;
using RaynMaker.Blade.Sdk;

namespace RaynMaker.Blade
{
    class Program
    {
        private static ILogger myLogger = LoggerFactory.GetLogger( typeof( Program ) );

        private static string myDataSheet;

        static void Main( string[] args )
        {
            try
            {
                ParseCommandLineArgs( args );

                var reader = new ValidatingXamlReader();
                var sheet = reader.Read<DataSheet>( myDataSheet );

                if( sheet.Asset is Stock )
                {
                    var analyzer = new StockAnalyzer( Console.Out );
                    analyzer.Execute( ( Stock )sheet.Asset );
                }
                else
                {
                    throw new NotSupportedException( "Asset type not supported: " + sheet.Asset.GetType() );
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
                else
                {
                    myDataSheet = args[ i ];
                }
            }

            Contract.Requires( myDataSheet != null, "No datasheet given" );
        }

        private static void Usage()
        {
            Console.WriteLine( "{0} [options] <datasheet>", Path.GetFileName( typeof( Program ).GetType().Assembly.Location ) );
            Console.WriteLine();
            Console.WriteLine( "Global options:" );
            Console.WriteLine( "  -h/--help              print usage" );
            Console.WriteLine();
        }
    }
}
