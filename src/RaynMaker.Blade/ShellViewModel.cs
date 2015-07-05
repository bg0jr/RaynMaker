using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Plainion;
using Plainion.Prism.Events;
using Plainion.Xaml;
using RaynMaker.Blade.AnalysisSpec;
using RaynMaker.Blade.DataSheetSpec;

namespace RaynMaker.Blade
{
    [Export]
    class ShellViewModel : BindableBase
    {
        private string myCurrenciesSheetLocation;
        private string myAnalysisTemplateLocation;
        private string myDataSheetLocation;

        [ImportingConstructor]
        public ShellViewModel( IEventAggregator eventAggregator )
        {
            GoCommand = new DelegateCommand( OnGo, CanGo );

            eventAggregator.GetEvent<ApplicationReadyEvent>().Subscribe( o => OnApplicationReady() );
        }

        private void OnApplicationReady()
        {
            ParseCommandLineArgs( Environment.GetCommandLineArgs() );
        }

        private void ParseCommandLineArgs( string[] args )
        {
            for( int i = 1; i < args.Length; ++i )
            {
                if( args[ i ] == "-h" || args[ i ] == "--help" )
                {
                    Usage();
                    Environment.Exit( 0 );
                }
                else if( args[ i ] == "-a" || args[ i ] == "--analysis" )
                {
                    Contract.Requires( i + 1 < args.Length, "-a requires an argument" );
                    i++;
                    AnalysisTemplateLocation = args[ i ];
                }
                else if( args[ i ] == "-c" || args[ i ] == "--currencies" )
                {
                    Contract.Requires( i + 1 < args.Length, "-c requires an argument" );
                    i++;
                    CurrenciesSheetLocation = args[ i ];
                }
                else
                {
                    DataSheetLocation = args[ i ];
                }
            }

            if( CurrenciesSheetLocation == null )
            {
                CurrenciesSheetLocation = Path.Combine( Path.GetDirectoryName( GetType().Assembly.Location ), "data", "Currencies.xaml" );
            }
        }

        private void Usage()
        {
            var sb = new StringBuilder();
            sb.AppendFormat( "{0} [options] <datasheet>", Path.GetFileName( GetType().GetType().Assembly.Location ) );
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine( "Options:" );
            sb.AppendLine( "  -h/--help                   print usage" );
            sb.AppendLine( "  -a/--analysis <file>        path to analysis sheet" );
            sb.AppendLine( "  -c/--currencies <file>      path to currencies sheet" );
            sb.AppendLine();

            MessageBox.Show( sb.ToString(), "Commandline usage", MessageBoxButton.OK, MessageBoxImage.Information );
        }

        public string CurrenciesSheetLocation
        {
            get { return myCurrenciesSheetLocation; }
            set
            {
                if( SetProperty( ref myCurrenciesSheetLocation, value ) )
                {
                    GoCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string AnalysisTemplateLocation
        {
            get { return myAnalysisTemplateLocation; }
            set
            {
                if( SetProperty( ref myAnalysisTemplateLocation, value ) )
                {
                    GoCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string DataSheetLocation
        {
            get { return myDataSheetLocation; }
            set
            {
                if( SetProperty( ref myDataSheetLocation, value ) )
                {
                    GoCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public DelegateCommand GoCommand { get; private set; }

        private bool CanGo()
        {
            return Exists( CurrenciesSheetLocation ) && Exists( AnalysisTemplateLocation ) && Exists( DataSheetLocation );
        }

        private bool Exists( string path )
        {
            return !string.IsNullOrEmpty( path ) && File.Exists( path );
        }

        private void OnGo()
        {
            var reader = new ValidatingXamlReader();

            Currencies.Sheet = reader.Read<CurrenciesSheet>( CurrenciesSheetLocation );
            var analysisTemplate = reader.Read<AnalysisTemplate>( AnalysisTemplateLocation );
            var dataSheet = reader.Read<DataSheet>( DataSheetLocation );

            if( dataSheet.Asset is Stock )
            {
                var analyzer = new StockAnalyzer( analysisTemplate.Analysis );
                analyzer.Execute( ( Stock )dataSheet.Asset );
            }
            else
            {
                throw new NotSupportedException( "Asset type not supported: " + dataSheet.Asset.GetType() );
            }
        }
    }
}
