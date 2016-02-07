using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
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
    class WebSpyViewModel : BindableBase, IInteractionRequestAware
    {
        private IDocumentBrowser myDocumentBrowser = null;
        private IProjectHost myProjectHost;
        private StorageService myStorageService;
        private INotification myNotification;

        [ImportingConstructor]
        public WebSpyViewModel( IProjectHost projectHost, StorageService storageService, ILutService lutService )
        {
            myProjectHost = projectHost;
            myStorageService = storageService;

            Session = new Session();

            DataSourcesNavigation = new DataSourcesNavigationViewModel( Session );

            SourceDefinition = new DataSourceDefinitionViewModel( Session );
            DocumentLocation = new DocumentLocationViewModel( Session );
            Figures = new DataSourceFiguresViewModel( Session, lutService );
            Validation = new ValidationViewModel( Session, myProjectHost, myStorageService, DataSourcesNavigation );
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

        public Session Session {get;private set;}

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
                Validation.Browser = myDocumentBrowser;

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

        public DataSourcesNavigationViewModel DataSourcesNavigation { get; private set; }

        public DataSourceDefinitionViewModel SourceDefinition { get; private set; }

        public DocumentLocationViewModel DocumentLocation { get; private set; }

        public DataSourceFiguresViewModel Figures { get; private set; }

        public ValidationViewModel Validation { get; private set; }

        public ICommand ResetCommand { get; private set; }

        private void OnReset()
        {
            Session.Reset();

            foreach( var source in myStorageService.Load() )
            {
                Session.Sources.Add( source );
            }

            Session.CurrentSource = Session.Sources.FirstOrDefault();
        }

        public ICommand SaveCommand { get; private set; }

        private void OnSave()
        {
            myStorageService.Store( Session.Sources );
        }

        public Action FinishInteraction { get; set; }

        public INotification Notification
        {
            get { return myNotification; }
            set
            {
                myNotification = value;
                
                // TODO: this is a workaround to get notified when the window is re-opened
                OnProjectChanged();
            }
        }
    }
}
