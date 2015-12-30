using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2;
using RaynMaker.Import.Spec.v2.Locating;
using RaynMaker.Import.Web.Model;
using RaynMaker.Import.Web.Services;
using RaynMaker.Import.WinForms;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Services;

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
        public WebSpyViewModel( IProjectHost projectHost, StorageService storageService, ILutService lutService )
        {
            myProjectHost = projectHost;
            myStorageService = storageService;

            mySession = new Session();

            SourceDefinition = new DataSourceDefinitionViewModel( mySession );
            Navigation = new NavigationViewModel( mySession );
            Formats = new DataFormatsViewModel( mySession, lutService );
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
                myDocumentBrowser = DocumentProcessorsFactory.CreateBrowser( value );

                // disable links
                // TODO: we cannot use this, it disables navigation in general (Navigate() too)
                //myBrowser.AllowNavigation = false;

                // TODO: how to disable images in browser

                Navigation.Browser = myDocumentBrowser;
                Formats.Browser = myDocumentBrowser;
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

        public NavigationViewModel Navigation { get; private set; }

        public DataFormatsViewModel Formats { get; private set; }

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
