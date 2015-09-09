using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Blade.Collections;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Plainion;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Web.Model;

namespace RaynMaker.Import.Web.ViewModels
{
    class NavigationViewModel : BindableBase
    {
        private Session mySession;
        private IBrowser myBrowser;
        private Site mySelectedSite;
        private string mySiteName;
        private DocumentType mySelectedDocumentType;
        private bool myIsCapturing;

        public NavigationViewModel( Session session )
        {
            Contract.RequiresNotNull( session, "session" );

            mySession = session;

            PropertyChangedEventManager.AddHandler( mySession, OnCurrentLocatorChanged, PropertySupport.ExtractPropertyName( () => mySession.CurrentLocator ) );

            AddSiteCommand = new DelegateCommand( OnAddSite );
            RemoveSiteCommand = new DelegateCommand( OnRemoveSite );

            CaptureCommand = new DelegateCommand( OnCapture );
            EditCommand = new DelegateCommand( OnEdit );

            EditCaptureRequest = new InteractionRequest<IConfirmation>();

            Sites = new ObservableCollection<Site>();
            Urls = new ObservableCollection<NavigatorUrl>();

            WeakEventManager<INotifyCollectionChanged, NotifyCollectionChangedEventArgs>.AddHandler( Urls, "CollectionChanged", OnUrlChanged );

            AddressBar = new AddressBarViewModel();

            OnCurrentLocatorChanged( null, null );
        }

        private void OnCurrentLocatorChanged( object sender, PropertyChangedEventArgs e )
        {
            Sites.Clear();

            if( mySession.CurrentLocator != null )
            {
                Sites.AddRange( mySession.CurrentLocator.Sites.OrderBy( s => s.Name ) );
            }

            SelectedSite = Sites.FirstOrDefault();
        }

        public IBrowser Browser
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
                Urls.Add( new NavigatorUrl( UriType.Request, url ) );
            }
        }

        private void BrowserDocumentCompleted( System.Windows.Forms.HtmlDocument doc )
        {
            AddressBar.Url = doc.Url.ToString();

            if( IsCapturing )
            {
                Urls.Add( new NavigatorUrl( UriType.Response, doc.Url ) );
            }
        }

        public ObservableCollection<Site> Sites { get; private set; }

        public Site SelectedSite
        {
            get { return mySelectedSite; }
            set
            {
                if( SetProperty( ref mySelectedSite, value ) )
                {
                    mySession.CurrentSite = mySelectedSite;

                    if( mySelectedSite != null )
                    {
                        // changing Urls property will automatically be reflected in mySelectedSite.Navigation.Uris.
                        // -> make a copy!
                        var modelUrls = mySelectedSite.Navigation.Uris.ToList();
                        Urls.Clear();
                        Urls.AddRange( modelUrls );

                        SelectedDocumentType = mySelectedSite.Navigation.DocumentType;
                        SiteName = mySelectedSite.Name;
                    }
                    else
                    {
                        Urls.Clear();
                        SelectedDocumentType = DocumentType.None;
                        SiteName = null;
                    }
                }
            }
        }

        public ICommand AddSiteCommand { get; private set; }

        private void OnAddSite()
        {
            var site = new Site( "unknown" );
            site.Navigation = new Navigation( DocumentType.Html );

            mySession.CurrentLocator.Sites.Add( site );

            // sorting ...
            Sites.Clear();
            Sites.AddRange( mySession.CurrentLocator.Sites.OrderBy( s => s.Name ) );

            SelectedSite = site;
        }

        public ICommand RemoveSiteCommand { get; private set; }

        private void OnRemoveSite()
        {
            var site = SelectedSite;

            Sites.Remove( site );
            SelectedSite = Sites.FirstOrDefault();

            mySession.CurrentLocator.Sites.Remove( site );
        }

        public string SiteName
        {
            get { return mySiteName; }
            set
            {
                if( SetProperty( ref mySiteName, value ) )
                {
                    if( SelectedSite != null && SelectedSite.Name != mySiteName )
                    {
                        SelectedSite.Name = mySiteName;

                        var old = SelectedSite;
                        SelectedSite = null;
                        
                        // sorting ...
                        Sites.Clear();
                        Sites.AddRange( mySession.CurrentLocator.Sites.OrderBy( s => s.Name ) );

                        // force update of caption of combobox
                        SelectedSite = old;
                    }
                }
            }
        }

        public DocumentType SelectedDocumentType
        {
            get { return mySelectedDocumentType; }
            set
            {
                if( SetProperty( ref mySelectedDocumentType, value ) )
                {
                    if( SelectedSite != null )
                    {
                        SelectedSite.Navigation.DocumentType = mySelectedDocumentType;
                    }
                }
            }
        }

        public ObservableCollection<NavigatorUrl> Urls { get; private set; }

        private void OnUrlChanged( object sender, NotifyCollectionChangedEventArgs e )
        {
            if( mySelectedSite != null )
            {
                mySelectedSite.Navigation.Uris.Clear();
                mySelectedSite.Navigation.Uris.AddRange( Urls );
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
                    Urls.AddRange( ( IEnumerable<NavigatorUrl> )c.Content );
                }
            } );
        }

        public InteractionRequest<IConfirmation> EditCaptureRequest { get; private set; }

        public AddressBarViewModel AddressBar { get; private set; }
    }
}
