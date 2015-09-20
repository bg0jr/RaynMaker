using System;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Web.Model;
using RaynMaker.Import.Web.Services;
using RaynMaker.Import.WinForms;
using RaynMaker.Infrastructure;

namespace RaynMaker.Import.Web.ViewModels
{
    [Export]
    class WebSpyViewModel : BindableBase
    {
        private IDocumentBrowser myDocumentBrowser = null;
        private IProjectHost myProjectHost;
        private StorageService myStorageService;
        private Session mySession;

        [ImportingConstructor]
        public WebSpyViewModel( IProjectHost projectHost, StorageService storageService )
        {
            myProjectHost = projectHost;
            myStorageService = storageService;

            mySession = new Session();

            SourceDefinition = new DataSourceDefinitionViewModel( mySession );
            Navigation = new NavigationViewModel( mySession );
            Formats = new DataFormatsViewModel( mySession );
            Completion = new CompletionViewModel( mySession, myProjectHost, myStorageService );
            
            myProjectHost.Changed += OnProjectChanged;
            OnProjectChanged();
        }

        private void OnProjectChanged()
        {
            if( myProjectHost.Project == null )
            {
                return;
            }

            mySession.Reset();

            foreach( var source in myStorageService.Load() )
            {
                mySession.Sources.Add( source );
            }

            //mySession.CurrentSource = mySession.Sources.FirstOrDefault();
        }

        public SafeWebBrowser Browser
        {
            set
            {
                myDocumentBrowser = DocumentProcessorsFactory.CreateBrowser( value );

                // disable links
                // TODO: we cannot use this, it disables navigation in general (Navigate() too)
                //myBrowser.AllowNavigation = false;

                // TODO: how to disable images in browser

                Navigation.Browser = myDocumentBrowser;
                Formats.Browser = myDocumentBrowser;
                Completion.Browser = myDocumentBrowser;

                myDocumentBrowser.Navigate( DocumentType.Html, new Uri( "http://www.google.com" ) );
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

        public NavigationViewModel Navigation { get; private set; }

        public DataFormatsViewModel Formats { get; private set; }

        public CompletionViewModel Completion { get; private set; }
    }
}
