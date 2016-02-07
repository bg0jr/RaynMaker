using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using Plainion.Collections;
using RaynMaker.Entities;
using RaynMaker.Infrastructure;
using RaynMaker.Modules.Import.Spec.v2;
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

            OnProjectChanged();
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
                }
            }
        }

        public DelegateCommand ValidateCommand { get; private set; }

        private bool CanValidate() { return SelectedStock != null; }

        private void OnValidate()
        {
            if( Session.CurrentSource == null )
            {
                return;
            }

            try
            {
                Browser.Navigate( DocumentType.Html, Session.CurrentSource.Location, new StockMacroResolver( SelectedStock ) );

                myValidationReport.NavigationSucceeded( Session.CurrentSource );
            }
            catch( Exception ex )
            {
                var sb = new StringBuilder();
                sb.AppendLine( ex.Message );
                
                foreach(var key in ex.Data.Keys )
                {
                    sb.AppendFormat( "{0}: {1}", key, ex.Data[ key ] );
                    sb.AppendLine();
                }

                myValidationReport.FailedToLocateDocument( Session.CurrentSource, sb.ToString() );

                return;
            }

            // The new document is automatically given to the selected FigureDescriptor ViewModel.
            // The MarkupBehavior gets automatically applied

            var parser = DocumentProcessingFactory.CreateParser( Browser.Document, Session.CurrentFigureDescriptor );

            try
            {
                var table = parser.ExtractTable();

                if( table.Rows.Count == 0 )
                {
                    myValidationReport.FailedToParseDocument( Session.CurrentFigureDescriptor, "Unknown reason" );
                }
                else
                {
                    myValidationReport.ParsingSucceeded( Session.CurrentFigureDescriptor );
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

                myValidationReport.FailedToParseDocument( Session.CurrentFigureDescriptor, sb.ToString() );
            }
        }
    }
}
