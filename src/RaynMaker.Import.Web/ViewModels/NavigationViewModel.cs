using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Plainion.Collections;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Web.Model;

namespace RaynMaker.Import.Web.ViewModels
{
    class NavigationViewModel : SpecDefinitionViewModelBase
    {
        private IDocumentBrowser myBrowser;
        private DocumentType mySelectedDocumentType;
        private bool myIsCapturing;

        public NavigationViewModel( Session session )
            : base( session )
        {
            PropertyChangedEventManager.AddHandler( Session, OnCurrentDataSourceChanged, PropertySupport.ExtractPropertyName( () => Session.CurrentSource ) );

            CaptureCommand = new DelegateCommand( OnCapture );
            EditCommand = new DelegateCommand( OnEdit );

            EditCaptureRequest = new InteractionRequest<IConfirmation>();

            Urls = new ObservableCollection<LocatingFragment>();

            WeakEventManager<INotifyCollectionChanged, NotifyCollectionChangedEventArgs>.AddHandler( Urls, "CollectionChanged", OnUrlChanged );

            AddressBar = new AddressBarViewModel();

            OnCurrentDataSourceChanged( null, null );
        }

        private void OnCurrentDataSourceChanged( object sender, PropertyChangedEventArgs e )
        {
            if( Session.CurrentSource != null )
            {
                // changing Urls property will automatically be reflected in mySelectedSite.Navigation.Uris.
                // -> make a copy!
                var modelUrls = Session.CurrentSource.LocationSpec.Uris.ToList();
                Urls.Clear();
                Urls.AddRange( modelUrls );

                SelectedDocumentType = Session.CurrentSource.LocationSpec.DocumentType;
            }
            else
            {
                Urls.Clear();
                SelectedDocumentType = DocumentType.None;
            }
        }

        public IDocumentBrowser Browser
        {
            get { return myBrowser; }
            set
            {
                var oldBrowser = myBrowser;
                if( SetProperty( ref myBrowser, value ) )
                {
                    AddressBar.Browser = myBrowser;

                    if( oldBrowser != null )
                    {
                        oldBrowser.Navigating -= OnBrowserNavigating;
                        oldBrowser.DocumentCompleted -= BrowserDocumentCompleted;
                    }
                    if( myBrowser != null )
                    {
                        myBrowser.Navigating += OnBrowserNavigating;
                        myBrowser.DocumentCompleted += BrowserDocumentCompleted;
                    }
                }
            }
        }

        private void OnBrowserNavigating( Uri url )
        {
            if( IsCapturing )
            {
                Urls.Add( new LocatingFragment( UriType.Request, url ) );
            }
        }

        private void BrowserDocumentCompleted( IDocument doc )
        {
            AddressBar.Url = doc.Location.ToString();

            if( IsCapturing )
            {
                Urls.Add( new LocatingFragment( UriType.Response, doc.Location ) );
            }
        }

        public DocumentType SelectedDocumentType
        {
            get { return mySelectedDocumentType; }
            set
            {
                if( SetProperty( ref mySelectedDocumentType, value ) )
                {
                    if( Session.CurrentSource != null )
                    {
                        Session.CurrentSource.LocationSpec.DocumentType = mySelectedDocumentType;
                    }
                }
            }
        }

        public ObservableCollection<LocatingFragment> Urls { get; private set; }

        private void OnUrlChanged( object sender, NotifyCollectionChangedEventArgs e )
        {
            if( Session.CurrentSource != null )
            {
                var old = Session.CurrentSource.LocationSpec;
                Session.CurrentSource.LocationSpec = new DocumentLocator( old.DocumentType, old.Uris.Concat( Urls ) );
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
            notification.Content = Urls;

            EditCaptureRequest.Raise( notification, c =>
            {
                if( c.Confirmed )
                {
                    Urls.Clear();
                    Urls.AddRange( ( IEnumerable<LocatingFragment> )c.Content );
                }
            } );
        }

        public InteractionRequest<IConfirmation> EditCaptureRequest { get; private set; }

        public AddressBarViewModel AddressBar { get; private set; }
    }
}
