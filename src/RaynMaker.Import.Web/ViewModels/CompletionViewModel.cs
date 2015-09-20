using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Plainion.Collections;
using RaynMaker.Entities;
using RaynMaker.Import.Web.Model;
using RaynMaker.Import.Web.Services;
using RaynMaker.Infrastructure;

namespace RaynMaker.Import.Web.ViewModels
{
    class CompletionViewModel : BindableBase
    {
        private Session mySession;
        private IProjectHost myProjectHost;
        private StorageService myStorageService;
        private Stock mySelectedStock;

        public CompletionViewModel( Session session, IProjectHost projectHost, StorageService storageService )
        {
            mySession = session;
            myProjectHost = projectHost;
            myProjectHost.Changed += OnProjectChanged;
            myStorageService = storageService;

            Stocks = new ObservableCollection<Stock>();

            ValidateCommand = new DelegateCommand( OnValidate, CanValidate );
            ClearCommand = new DelegateCommand( OnClear );
            SaveCommand = new DelegateCommand( OnSave );

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
            if( mySession.CurrentSource == null )
            {
                return;
            }

            var provider = new BasicDatumProvider( Browser );
            provider.Navigate( mySession.CurrentSource, SelectedStock );
            
            // do not use Mark() API ... it creates markup which will not be removed again
            //provider.Mark( mySession.CurrentFormat );

            if( mySession.ApplyCurrentFormat != null )
            {
                mySession.ApplyCurrentFormat();
            }
        }

        public ICommand ClearCommand { get; private set; }

        private void OnClear()
        {
            mySession.Reset();
        }

        public ICommand SaveCommand { get; private set; }

        private void OnSave()
        {
            myStorageService.Store( mySession.Locators );
        }
    }
}
