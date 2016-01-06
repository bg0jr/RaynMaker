using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Plainion.Collections;
using RaynMaker.Entities;
using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Web.Model;
using RaynMaker.Modules.Import.Web.Services;
using RaynMaker.Infrastructure;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class CompletionViewModel : SpecDefinitionViewModelBase
    {
        private IProjectHost myProjectHost;
        private StorageService myStorageService;
        private Stock mySelectedStock;

        public CompletionViewModel( Session session, IProjectHost projectHost, StorageService storageService )
            : base( session )
        {
            myProjectHost = projectHost;
            myProjectHost.Changed += OnProjectChanged;
            myStorageService = storageService;

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

            Browser.Navigate( DocumentType.Html, Session.CurrentSource.Location, new StockMacroResolver( SelectedStock ) );

            // do not use own marker here - there is not point in time defined when Marker.Unmark() could be called and so the 
            // markup would remain. We would need to change user navigation to add some transition like "leave validation"
            if( Session.ApplyCurrentFormat != null )
            {
                Session.ApplyCurrentFormat();
            }
        }
    }
}
