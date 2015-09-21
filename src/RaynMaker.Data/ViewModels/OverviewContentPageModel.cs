using System.ComponentModel.Composition;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Plainion.Windows.Controls;
using RaynMaker.Data.Services;
using RaynMaker.Entities;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Services;

namespace RaynMaker.Data.ViewModels
{
    [Export]
    class OverviewContentPageModel : BindableBase, IContentPage
    {
        private IProjectHost myProjectHost;
        private StorageService myStorageService;
        private Stock myStock;
        private FlowDocument myNotes;

        [ImportingConstructor]
        public OverviewContentPageModel( IProjectHost projectHost, StorageService storageService )
        {
            myProjectHost = projectHost;
            myStorageService = storageService;

            AddReferenceCommand = new DelegateCommand( OnAddReference );
            RemoveReferenceCommand = new DelegateCommand<Reference>( OnRemoveReference );
        }

        public string Header { get { return "Overview"; } }

        public Stock Stock
        {
            get { return myStock; }
            set { SetProperty( ref myStock, value ); }
        }

        public void Initialize( Stock stock )
        {
            Stock = stock;

            if( Notes != null )
            {
                myStorageService.Load( Stock, Notes );
            }
        }

        public void Complete()
        {
            TextBoxBinding.ForceSourceUpdate();

            var ctx = myProjectHost.Project.GetAssetsContext();
            ctx.SaveChanges();

            myStorageService.Store( myStock, Notes );
        }

        public void Cancel()
        {
        }

        public ICommand AddReferenceCommand { get; private set; }

        private void OnAddReference()
        {
            Stock.Company.References.Add( new Reference() );
        }

        public ICommand RemoveReferenceCommand { get; private set; }

        private void OnRemoveReference( Reference reference )
        {
            Stock.Company.References.Remove( reference );
        }

        public FlowDocument Notes
        {
            get { return myNotes; }
            set
            {
                if( SetProperty( ref myNotes, value ) )
                {
                    if( Stock != null )
                    {
                        myStorageService.Load( Stock, Notes );
                    }
                }
            }
        }
    }
}
