using System.ComponentModel.Composition;
using System.Windows.Documents;
using Microsoft.Practices.Prism.Mvvm;
using Plainion.Windows.Controls;
using RaynMaker.Entities;
using RaynMaker.Infrastructure;

namespace RaynMaker.Modules.Notes.ViewModels
{
    [Export]
    class NotesContentPageModel : BindableBase, IContentPage
    {
        private IProjectHost myProjectHost;
        private StorageService myStorageService;
        private Stock myStock;
        private FlowDocument myDocument;

        [ImportingConstructor]
        public NotesContentPageModel( IProjectHost projectHost, StorageService storageService )
        {
            myProjectHost = projectHost;
            myStorageService = storageService;
        }

        public string Header { get { return "Notes"; } }

        public Stock Stock
        {
            get { return myStock; }
            set { SetProperty( ref myStock, value ); }
        }

        public void Initialize( Stock stock )
        {
            Stock = stock;

            if( Document != null )
            {
                myStorageService.Load( Stock, Document );
            }
        }

        public void Complete()
        {
            TextBoxBinding.ForceSourceUpdate();

            myStorageService.Store( myStock, Document );
        }

        public void Cancel()
        {
        }

        public FlowDocument Document
        {
            get { return myDocument; }
            set
            {
                if( SetProperty( ref myDocument, value ) )
                {
                    if( Stock != null )
                    {
                        myStorageService.Load( Stock, Document );
                    }
                }
            }
        }
    }
}
