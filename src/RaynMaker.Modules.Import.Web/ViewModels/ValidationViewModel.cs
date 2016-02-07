using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using Plainion.Collections;
using RaynMaker.Entities;
using RaynMaker.Infrastructure;
using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.Modules.Import.Web.Model;
using RaynMaker.Modules.Import.Web.Services;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class ValidationViewModel : SpecDefinitionViewModelBase
    {
        private IProjectHost myProjectHost;
        private StorageService myStorageService;
        private Stock mySelectedStock;
        private INotifyValidationFailed myValidationReport;

        public ValidationViewModel( Session session, IProjectHost projectHost, StorageService storageService, INotifyValidationFailed validationReport )
            : base( session )
        {
            myProjectHost = projectHost;
            myProjectHost.Changed += OnProjectChanged;
            myStorageService = storageService;
            myValidationReport = validationReport;

            Stocks = new ObservableCollection<Stock>();

            ValidateCommand = new DelegateCommand( OnValidate, CanValidate );
            ValidateAllCommand = new DelegateCommand( OnValidateAll, CanValidateAll );

            OnProjectChanged();

            PropertyChangedEventManager.AddHandler( Session, OnSessionChanged, "" );
            OnSessionChanged( null, null );
        }

        private void OnSessionChanged( object sender, PropertyChangedEventArgs e )
        {
            ValidateAllCommand.RaiseCanExecuteChanged();
        }
        
        private void OnProjectChanged()
        {
            if( myProjectHost.Project == null )
            {
                return;
            }

            var ctx = myProjectHost.Project.GetAssetsContext();
            Stocks.AddRange( ctx.Stocks );
            SelectedStock = Stocks.FirstOrDefault();
        }

        public IDocumentBrowser Browser { get; set; }

        public ObservableCollection<Stock> Stocks { get; private set; }

        public Stock SelectedStock
        {
            get { return mySelectedStock; }
            set
            {
                if( SetProperty( ref mySelectedStock, value ) )
                {
                    ValidateCommand.RaiseCanExecuteChanged();
                    ValidateAllCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public DelegateCommand ValidateCommand { get; private set; }

        private bool CanValidate() { return SelectedStock != null && Session.CurrentSource != null; }

        private void OnValidate()
        {
            Validate( Session.CurrentSource, Session.CurrentFigureDescriptor );
        }

        private void Validate(DataSource source, IFigureDescriptor figureDescriptor)
        {
            try
            {
                Browser.Navigate( DocumentType.Html, source.Location, new StockMacroResolver( SelectedStock ) );

                myValidationReport.NavigationSucceeded( source );
            }
            catch( Exception ex )
            {
                var sb = new StringBuilder();
                sb.AppendLine( ex.Message );

                foreach( var key in ex.Data.Keys )
                {
                    sb.AppendFormat( "{0}: {1}", key, ex.Data[ key ] );
                    sb.AppendLine();
                }

                myValidationReport.FailedToLocateDocument( source, sb.ToString() );

                return;
            }

            // The new document is automatically given to the selected FigureDescriptor ViewModel.
            // The MarkupBehavior gets automatically applied

            var parser = DocumentProcessingFactory.CreateParser( Browser.Document, figureDescriptor );

            try
            {
                var table = parser.ExtractTable();

                if( table.Rows.Count == 0 )
                {
                    myValidationReport.FailedToParseDocument( figureDescriptor, "Unknown reason" );
                }
                else
                {
                    myValidationReport.ParsingSucceeded( figureDescriptor );
                }
            }
            catch( Exception ex )
            {
                var sb = new StringBuilder();
                sb.AppendLine( ex.Message );

                foreach( var key in ex.Data.Keys )
                {
                    sb.AppendFormat( "{0}: {1}", key, ex.Data[ key ] );
                    sb.AppendLine();
                }

                myValidationReport.FailedToParseDocument( figureDescriptor, sb.ToString() );
            }
        }

        public DelegateCommand ValidateAllCommand { get; private set; }

        private bool CanValidateAll() { return Session.Sources.Any(); }

        private void OnValidateAll()
        {
            foreach( var source in Session.Sources )
            {
                foreach( var figure in source.Figures )
                {
                    // TODO: async + parallel
                    Validate( source, figure );
                }
            }
        }
    }
}
