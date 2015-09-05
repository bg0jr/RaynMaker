using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
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
        private Site mySelectedSite;
        private string mySiteName;
        private DocumentType mySelectedDocumentType;
        private bool myIsCapturing;

        public NavigationViewModel( Session session )
        {
            Contract.RequiresNotNull( session, "session" );

            mySession = session;

            PropertyChangedEventManager.AddHandler( mySession, OnCurrentLocatorChanged, PropertySupport.ExtractPropertyName( () => mySession.CurrentLocator ) );
            OnCurrentLocatorChanged( null, null );

            AddSiteCommand = new DelegateCommand( OnAddSite );
            RemoveSiteCommand = new DelegateCommand( OnRemoveSite );

            CaptureCommand = new DelegateCommand( OnCapture );
            ReplayCommand = new DelegateCommand( OnReplay );
            EditCommand = new DelegateCommand( OnEdit );

            EditCaptureRequest = new InteractionRequest<IConfirmation>();
            InputMacroValueRequest = new InteractionRequest<IConfirmation>();

            Urls = new ObservableCollection<NavigatorUrl>();

            WeakEventManager<INotifyCollectionChanged, NotifyCollectionChangedEventArgs>.AddHandler( Urls, "CollectionChanged", OnUrlChanged );
        }

        private void OnCurrentLocatorChanged( object sender, PropertyChangedEventArgs e )
        {
            if( mySession.CurrentLocator != null )
            {
                Sites = new ObservableCollection<Site>( mySession.CurrentLocator.Sites );
                SelectedSite = Sites.FirstOrDefault();
            }
            else
            {
                Sites = new ObservableCollection<Site>();
                SelectedSite = Sites.FirstOrDefault();
            }
        }

        public IBrowser Browser { get; set; }

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
                        mySelectedDocumentType = mySelectedSite.Navigation.DocumentType;
                        Urls.Clear();
                        Urls.AddRange( mySelectedSite.Navigation.Uris );
                        SiteName = mySelectedSite.Name;
                    }
                    else
                    {
                        mySelectedDocumentType = DocumentType.None;
                        mySiteName = null;
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

            Sites.Add( site );
            SelectedSite = site;
        }

        public ICommand RemoveSiteCommand { get; private set; }

        private void OnRemoveSite()
        {
            var site = SelectedSite;

            Sites.Remove( site );
            SelectedSite = Sites.FirstOrDefault();

            mySession.CurrentLocator.Sites.Add( site );
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

                        // force refresh of selected site to force update of combobox
                        var old = SelectedSite;
                        SelectedSite = null;
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
                    SelectedSite.Navigation.DocumentType = mySelectedDocumentType;
                }
            }
        }

        public ObservableCollection<NavigatorUrl> Urls { get; private set; }

        private void OnUrlChanged( object sender, NotifyCollectionChangedEventArgs e )
        {
            mySelectedSite.Navigation.Uris = Urls;
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

        public ICommand ReplayCommand { get; private set; }

        private void OnReplay()
        {
            var macroPattern = new Regex( @"(\$\{.*\})" );
            var filtered = new List<NavigatorUrl>();
            foreach( var navUrl in Urls )
            {
                var md = macroPattern.Match( navUrl.UrlString );
                if( md.Success )
                {
                    string macro = md.Groups[ 1 ].Value;
                    string value = GetValue( macro );
                    if( value != null )
                    {
                        filtered.Add( new NavigatorUrl( navUrl.UrlType, navUrl.UrlString.Replace( macro, value ) ) );
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    filtered.Add( navUrl );
                }
            }

            Browser.LoadDocument( filtered );
        }

        private string GetValue( string macro )
        {
            var notification = new Confirmation();
            notification.Title = "Enter macro value";
            notification.Content = macro;

            string result = null;

            InputMacroValueRequest.Raise( notification, c =>
            {
                if( c.Confirmed )
                {
                    result = ( string )c.Content;
                }
            } );

            return result;
        }

        public InteractionRequest<IConfirmation> InputMacroValueRequest { get; private set; }

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
    }
}
