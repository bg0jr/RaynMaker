using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Plainion;
using Plainion.Prism.Events;
using Plainion.Prism.Interactivity.InteractionRequest;
using Plainion.Xaml;
using RaynMaker.Blade.AnalysisSpec;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.Entities;
using RaynMaker.Blade.Model;
using RaynMaker.Blade.Services;

namespace RaynMaker.Blade
{
    [Export]
    class ShellViewModel : BindableBase
    {
        private StorageService myStorageService;

        [ImportingConstructor]
        public ShellViewModel( IEventAggregator eventAggregator, Project project, StorageService storageService )
        {
            Project = project;
            myStorageService = storageService;

            PropertyChangedEventManager.AddHandler( Project, OnProjectPropertyChanged, string.Empty );

            GoCommand = new DelegateCommand( OnGo, CanGo );

            BrowseCurrenciesSheetLocationCommand = new DelegateCommand( OnBrowseCurrenciesSheetLocation );
            BrowseCurrenciesSheetLocationRequest = new InteractionRequest<OpenFileDialogNotification>();
            EditCurrenciesSheetCommand = new DelegateCommand( OnEditCurrencies );
            EditCurrenciesSheetRequest = new InteractionRequest<INotification>();

            BrowseAnalysisTemplateLocationCommand = new DelegateCommand( OnBrowseAnalysisTemplateLocation );
            BrowseAnalysisTemplateLocationRequest = new InteractionRequest<OpenFileDialogNotification>();
            EditAnalysisTemplateCommand = new DelegateCommand( OnEditAnalysisTemplate );
            EditAnalysisTemplateRequest = new InteractionRequest<INotification>();

            BrowseDataSheetLocationCommand = new DelegateCommand( OnBrowseDataSheetLocation );
            BrowseDataSheetLocationRequest = new InteractionRequest<OpenFileDialogNotification>();
            EditDataSheetCommand = new DelegateCommand( OnEditDataSheet );
            EditDataSheetRequest = new InteractionRequest<INotification>();

            eventAggregator.GetEvent<ApplicationReadyEvent>().Subscribe( o => OnApplicationReady() );
        }

        private void OnProjectPropertyChanged( object sender, PropertyChangedEventArgs e )
        {
            if( e.PropertyName == PropertySupport.ExtractPropertyName( () => Project.AnalysisTemplateLocation ) ||
                e.PropertyName == PropertySupport.ExtractPropertyName( () => Project.CurrenciesSheetLocation ) ||
                e.PropertyName == PropertySupport.ExtractPropertyName( () => Project.DataSheetLocation ) )
            {
                GoCommand.RaiseCanExecuteChanged();
            }
        }

        public Project Project { get; private set; }

        private void OnApplicationReady()
        {
            ParseCommandLineArgs( Environment.GetCommandLineArgs() );
        }

        private void ParseCommandLineArgs( string[] args )
        {
            string dataSheet = null;

            for( int i = 1; i < args.Length; ++i )
            {
                if( args[ i ] == "-h" || args[ i ] == "-help" )
                {
                    Usage();
                    Environment.Exit( 0 );
                }
                else if( args[ i ] == "-a" || args[ i ] == "-analysis" )
                {
                    Contract.Requires( i + 1 < args.Length, "-a requires an argument" );
                    i++;
                    Project.AnalysisTemplateLocation = args[ i ];
                }
                else if( args[ i ] == "-c" || args[ i ] == "-currencies" )
                {
                    Contract.Requires( i + 1 < args.Length, "-c requires an argument" );
                    i++;
                    Project.CurrenciesSheetLocation = args[ i ];
                }
                else
                {
                    dataSheet = args[ i ];
                }
            }

            if( Project.CurrenciesSheetLocation == null )
            {
                Project.CurrenciesSheetLocation = Path.Combine( Path.GetDirectoryName( GetType().Assembly.Location ), "Resources", "Currencies.xdb" );
            }

            // DataSheet has to be set after currencies are loaded because setting it here causes automatic loading which 
            // requires currency to be loaded
            Project.DataSheetLocation = dataSheet;
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

        public DelegateCommand GoCommand { get; private set; }

        private bool CanGo()
        {
            return Exists( Project.CurrenciesSheetLocation ) && Exists( Project.AnalysisTemplateLocation ) && Exists( Project.DataSheetLocation );
        }

        private bool Exists( string path )
        {
            return !string.IsNullOrEmpty( path ) && File.Exists( path );
        }

        private void OnGo()
        {
            var reader = new ValidatingXamlReader();

            Currencies.Sheet = myStorageService.LoadCurrencies( Project.CurrenciesSheetLocation );
            var analysisTemplate = reader.Read<AnalysisTemplate>( Project.AnalysisTemplateLocation );
            var dataSheet = myStorageService.LoadDataSheet( Project.DataSheetLocation );
            dataSheet.Freeze();

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

        public DelegateCommand BrowseCurrenciesSheetLocationCommand { get; private set; }

        private void OnBrowseCurrenciesSheetLocation()
        {
            var notification = new OpenFileDialogNotification();
            notification.RestoreDirectory = true;
            notification.Filter = "XDB files (*.xdb)|*.xdb";
            notification.FilterIndex = 0;
            notification.MultiSelect = false;

            BrowseCurrenciesSheetLocationRequest.Raise( notification,
                n =>
                {
                    if( n.Confirmed )
                    {
                        Project.CurrenciesSheetLocation = n.FileName;
                    }
                } );
        }

        public InteractionRequest<OpenFileDialogNotification> BrowseCurrenciesSheetLocationRequest { get; private set; }

        public DelegateCommand EditCurrenciesSheetCommand { get; private set; }

        private void OnEditCurrencies()
        {
            var notification = new Notification();
            notification.Title = "Edit Currencies";

            EditCurrenciesSheetRequest.Raise( notification, n => { } );
        }

        public InteractionRequest<INotification> EditCurrenciesSheetRequest { get; private set; }

        public DelegateCommand BrowseAnalysisTemplateLocationCommand { get; private set; }

        private void OnBrowseAnalysisTemplateLocation()
        {
            var notification = new OpenFileDialogNotification();
            notification.RestoreDirectory = true;
            notification.Filter = "XAML files (*.xaml)|*.xaml";
            notification.FilterIndex = 0;
            notification.MultiSelect = false;

            BrowseAnalysisTemplateLocationRequest.Raise( notification,
                n =>
                {
                    if( n.Confirmed )
                    {
                        Project.AnalysisTemplateLocation = n.FileName;
                    }
                } );
        }

        public InteractionRequest<OpenFileDialogNotification> BrowseAnalysisTemplateLocationRequest { get; private set; }

        public DelegateCommand EditAnalysisTemplateCommand { get; private set; }

        private void OnEditAnalysisTemplate()
        {
            var notification = new Notification();
            notification.Title = "Edit analysis template";

            EditAnalysisTemplateRequest.Raise( notification, n => { } );
        }

        public InteractionRequest<INotification> EditAnalysisTemplateRequest { get; private set; }

        public DelegateCommand BrowseDataSheetLocationCommand { get; private set; }

        private void OnBrowseDataSheetLocation()
        {
            var notification = new OpenFileDialogNotification();
            notification.RestoreDirectory = true;
            notification.Filter = "XAML files (*.xaml)|*.xaml";
            notification.FilterIndex = 0;
            notification.MultiSelect = false;

            BrowseDataSheetLocationRequest.Raise( notification,
                n =>
                {
                    if( n.Confirmed )
                    {
                        Project.DataSheetLocation = n.FileName;
                    }
                } );
        }

        public InteractionRequest<OpenFileDialogNotification> BrowseDataSheetLocationRequest { get; private set; }

        public DelegateCommand EditDataSheetCommand { get; private set; }

        private void OnEditDataSheet()
        {
            var notification = new Notification();
            notification.Title = "Edit data sheet";

            EditDataSheetRequest.Raise( notification, n => { } );
        }

        public InteractionRequest<INotification> EditDataSheetRequest { get; private set; }
    }
}
