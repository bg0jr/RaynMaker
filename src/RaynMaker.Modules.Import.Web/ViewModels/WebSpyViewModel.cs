using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Services;
using RaynMaker.Modules.Import.Design;
using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Web.Model;
using RaynMaker.Modules.Import.Web.Services;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    [Export]
    class WebSpyViewModel : BindableBase
    {
        private IDocumentBrowser myDocumentBrowser = null;
        private IProjectHost myProjectHost;
        private StorageService myStorageService;
        private Session mySession;

        [ImportingConstructor]
        public WebSpyViewModel( IProjectHost projectHost, StorageService storageService, ILutService lutService )
        {
            myProjectHost = projectHost;
            myStorageService = storageService;

            mySession = new Session();

            SourceDefinition = new DataSourceDefinitionViewModel( mySession );
            DocumentLocation = new DocumentLocationViewModel( mySession );
            Figures = new DataSourceFiguresViewModel( mySession, lutService );
            Completion = new CompletionViewModel( mySession, myProjectHost, myStorageService );

            ResetCommand = new DelegateCommand( OnReset );
            SaveCommand = new DelegateCommand( OnSave );
            
            myProjectHost.Changed += OnProjectChanged;
            OnProjectChanged();
        }

        private void OnProjectChanged()
        {
            if( myProjectHost.Project == null )
            {
                return;
            }

            OnReset();
        }

        public SafeWebBrowser Browser
        {
            set
            {
                myDocumentBrowser = DocumentProcessingFactory.CreateBrowser( value );

                // disable links
                // TODO: we cannot use this, it disables navigation in general (Navigate() too)
                //myBrowser.AllowNavigation = false;

                // TODO: how to disable images in browser

                DocumentLocation.Browser = myDocumentBrowser;
                Figures.Browser = myDocumentBrowser;
                Completion.Browser = myDocumentBrowser;

                myDocumentBrowser.Navigate( DocumentType.Html, new Uri( "about:blank" ) );
            }
        }

        //protected override void OnHandleDestroyed( EventArgs e )
        //{
        //    myMarkupDocument.Dispose();
        //    myMarkupDocument = null;

        //    myDocumentBrowser.Browser.DocumentCompleted -= myBrowser_DocumentCompleted;
        //    myDocumentBrowser.Dispose();
        //    myBrowser.Dispose();
        //    myBrowser = null;

        //    base.OnHandleDestroyed( e );
        //}

        public DataSourceDefinitionViewModel SourceDefinition { get; private set; }

        public DocumentLocationViewModel DocumentLocation { get; private set; }

        public DataSourceFiguresViewModel Figures { get; private set; }

        public CompletionViewModel Completion { get; private set; }
      
        public ICommand ResetCommand { get; private set; }

        private void OnReset()
        {
            mySession.Reset();

            foreach( var source in myStorageService.Load() )
            {
                mySession.Sources.Add( source );
            }

            mySession.CurrentSource = mySession.Sources.FirstOrDefault();
        }

        public ICommand SaveCommand { get; private set; }

        private void OnSave()
        {
            myStorageService.Store( mySession.Sources );
        }
    }
}
