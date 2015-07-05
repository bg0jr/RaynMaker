﻿using System;
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

        private static string myAnalysisSheet;
        private static string myDataSheet;
        private static string myCurrenciesSheet;

        [STAThread]
        static void Main( string[] args )
        {
            try
            {
                ParseCommandLineArgs( args );

                var reader = new ValidatingXamlReader();

                Currencies.Sheet = reader.Read<CurrenciesSheet>( myCurrenciesSheet );
                var analysisTemplate = reader.Read<AnalysisTemplate>( myAnalysisSheet );
                var dataSheet = reader.Read<DataSheet>( myDataSheet );

                if( dataSheet.Asset is Stock )
                {
                    var analyzer = new StockAnalyzer( analysisTemplate.Analysis, Console.Out );
                    analyzer.Execute( ( Stock )dataSheet.Asset );
                }
                else
                {
                    throw new NotSupportedException( "Asset type not supported: " + dataSheet.Asset.GetType() );
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
                    myAnalysisSheet = args[ i ];
                }
                else if( args[ i ] == "-c" || args[ i ] == "--currencies" )
                {
                    Contract.Requires( i + 1 < args.Length, "-c requires an argument" );
                    i++;
                    myCurrenciesSheet = args[ i ];
                }
                else
                {
                    myDataSheet = args[ i ];
                }
            }

            Contract.Requires( myAnalysisSheet != null, "No analysis sheet given" );
            Contract.Requires( myDataSheet != null, "No datasheet given" );

            if( myCurrenciesSheet == null )
            {
                myCurrenciesSheet = Path.Combine( Path.GetDirectoryName( typeof( Program ).Assembly.Location ), "data", "Currencies.xaml" );
            }
        }

        private static void Usage()
        {
            Console.WriteLine( "{0} [options] <datasheet>", Path.GetFileName( typeof( Program ).GetType().Assembly.Location ) );
            Console.WriteLine();
            Console.WriteLine( "Options:" );
            Console.WriteLine( "  -h/--help                   print usage" );
            Console.WriteLine( "  -a/--analysis <file>        path to analysis sheet" );
            Console.WriteLine( "  -c/--currencies <file>      path to currencies sheet" );
            Console.WriteLine();
        }
    }
}
