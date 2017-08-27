using System.ComponentModel;
using Prism.Mvvm;
using RaynMaker.Infrastructure.Services;
using RaynMaker.Modules.Import.Web.Model;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class DataSourceFiguresViewModel : SpecDefinitionViewModelBase
    {
        private IDescriptorViewModel mySelectedDescriptor;
        private IDocument myDocument;
        private IDocumentBrowser myBrowser;
        private FigureDescriptorViewModelFactory myDescriptorViewModelFactory;

        public DataSourceFiguresViewModel( Session session, ILutService lutService )
            : base( session )
        {
            myDescriptorViewModelFactory = new FigureDescriptorViewModelFactory( lutService );

            PropertyChangedEventManager.AddHandler( Session, OnFigureDescriptorChanged, PropertySupport.ExtractPropertyName( () => Session.CurrentFigureDescriptor ) );

            OnFigureDescriptorChanged( null, null );
        }

        private void OnFigureDescriptorChanged( object sender, PropertyChangedEventArgs e )
        {
            SelectedDescriptor = Session.CurrentFigureDescriptor == null
                ? null
                : myDescriptorViewModelFactory.Create( Session.CurrentFigureDescriptor );
        }

        public IDocumentBrowser Browser
        {
            get { return myBrowser; }
            set
            {
                var oldBrowser = myBrowser;
                if( SetProperty( ref myBrowser, value ) )
                {
                    if( oldBrowser != null )
                    {
                        oldBrowser.DocumentCompleted -= BrowserDocumentCompleted;
                    }
                    if( myBrowser != null )
                    {
                        myBrowser.DocumentCompleted += BrowserDocumentCompleted;
                    }
                }
            }
        }

        private void BrowserDocumentCompleted( IDocument doc )
        {
            Document = doc;
        }

        public IDescriptorViewModel SelectedDescriptor
        {
            get { return mySelectedDescriptor; }
            set
            {
                var oldDescriptor = mySelectedDescriptor;
                if( SetProperty( ref mySelectedDescriptor, value ) )
                {
                    if( oldDescriptor != null )
                    {
                        oldDescriptor.Unmark();
                        oldDescriptor.Document = null;
                    }

                    if( mySelectedDescriptor != null )
                    {
                        mySelectedDescriptor.Document = myDocument;
                    }
                }
            }
        }

        public IDocument Document
        {
            get { return myDocument; }
            set
            {
                // always force update because the document reference does NOT change!

                myDocument = value;

                if( SelectedDescriptor != null )
                {
                    SelectedDescriptor.Document = myDocument;
                }
            }
        }
    }
}
