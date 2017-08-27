using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Plainion.Collections;
using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Spec.v2.Locating;
using RaynMaker.Modules.Import.Web.Model;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class DocumentLocationViewModel : SpecDefinitionViewModelBase
    {
        private IDocumentBrowser myBrowser;
        private DataSource myDataSource;
        private bool myIsCapturing;

        public DocumentLocationViewModel( Session session )
            : base( session )
        {
            PropertyChangedEventManager.AddHandler( Session, OnCurrentDataSourceChanged, PropertySupport.ExtractPropertyName( () => Session.CurrentSource ) );

            CaptureCommand = new DelegateCommand( OnCapture );
            EditCommand = new DelegateCommand( OnEdit );

            EditCaptureRequest = new InteractionRequest<IConfirmation>();

            AddressBar = new AddressBarViewModel();

            OnCurrentDataSourceChanged( null, null );
        }

        public DataSource DataSource
        {
            get { return myDataSource; }
            private set { SetProperty( ref myDataSource, value ); }
        }

        private void OnCurrentDataSourceChanged( object sender, PropertyChangedEventArgs e )
        {
            DataSource = Session.CurrentSource;

            if ( DataSource != null && DataSource.Location == null )
            {
                DataSource.Location = new DocumentLocator();
            }
        }

        public IDocumentBrowser Browser
        {
            get { return myBrowser; }
            set
            {
                var oldBrowser = myBrowser;
                if ( SetProperty( ref myBrowser, value ) )
                {
                    AddressBar.Browser = myBrowser;

                    if ( oldBrowser != null )
                    {
                        oldBrowser.Navigating -= OnBrowserNavigating;
                        oldBrowser.DocumentCompleted -= BrowserDocumentCompleted;
                    }
                    if ( myBrowser != null )
                    {
                        myBrowser.Navigating += OnBrowserNavigating;
                        myBrowser.DocumentCompleted += BrowserDocumentCompleted;
                    }
                }
            }
        }

        private void OnBrowserNavigating( Uri url )
        {
            if ( IsCapturing )
            {
                DataSource.Location.Fragments.Add( new Request( url ) );
            }
        }

        private void BrowserDocumentCompleted( IDocument doc )
        {
            AddressBar.Url = doc.Location.ToString();

            if ( IsCapturing )
            {
                DataSource.Location.Fragments.Add( new Response( doc.Location ) );
            }
        }

        public bool IsCapturing
        {
            get { return myIsCapturing; }
            set { SetProperty( ref myIsCapturing, value ); }
        }

        public ICommand CaptureCommand { get; private set; }

        private void OnCapture()
        {
            IsCapturing = !IsCapturing;
        }

        public ICommand EditCommand { get; private set; }

        private void OnEdit()
        {
            var notification = new Confirmation();
            notification.Title = "Edit capture";
            notification.Content = DataSource.Location.Fragments;

            EditCaptureRequest.Raise( notification, c =>
            {
                if ( c.Confirmed )
                {
                    DataSource.Location.Fragments.Clear();
                    DataSource.Location.Fragments.AddRange( (IEnumerable<DocumentLocationFragment>)c.Content );
                }
            } );
        }

        public InteractionRequest<IConfirmation> EditCaptureRequest { get; private set; }

        public AddressBarViewModel AddressBar { get; private set; }
    }
}
