using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Import.Core;
using RaynMaker.Import.Html;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Web.Model;
using RaynMaker.Import.Web.Services;
using RaynMaker.Infrastructure;

namespace RaynMaker.Import.Web.ViewModels
{
    [Export]
    class WebSpyViewModel : BindableBase, IBrowser
    {
        private LegacyDocumentBrowser myDocumentBrowser = null;

        [ImportingConstructor]
        public WebSpyViewModel( IProjectHost projectHost, StorageService storageService )
        {
            var session = new Session();

            foreach( var locator in storageService.Load() )
            {
                session.AddLocator( locator );
            }

            session.CurrentLocator = session.Locators.FirstOrDefault();


            Datums = new DatumSelectionViewModel( session );
            Navigation = new NavigationViewModel( session );
            Formats = new DataFormatsViewModel( session );
            Completion = new CompletionViewModel( session, projectHost, storageService );
        }

        public System.Windows.Forms.WebBrowser Browser
        {
            set
            {
                myDocumentBrowser = new LegacyDocumentBrowser( value );
                myDocumentBrowser.Browser.Navigating += myBrowser_Navigating;
                myDocumentBrowser.Browser.DocumentCompleted += myBrowser_DocumentCompleted;

                // disable links
                // TODO: we cannot use this, it disables navigation in general (Navigate() too)
                //myBrowser.AllowNavigation = false;

                // TODO: how to disable images in browser

                Navigation.Browser = this;
                Formats.Browser = this;
                Completion.Browser = this;

                Navigate( "http://www.google.com" );
            }
        }

        private void myBrowser_Navigating( object sender, System.Windows.Forms.WebBrowserNavigatingEventArgs e )
        {
            if( Navigating != null )
            {
                Navigating( e.Url );
            }
        }

        private void myBrowser_DocumentCompleted( object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e )
        {
            if( DocumentCompleted != null )
            {
                DocumentCompleted( myDocumentBrowser.Document );
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

        public DatumSelectionViewModel Datums { get; private set; }

        public NavigationViewModel Navigation { get; private set; }

        public DataFormatsViewModel Formats { get; private set; }

        public CompletionViewModel Completion { get; private set; }

        public void Navigate( string url )
        {
            myDocumentBrowser.Navigate( url );
        }

        public IHtmlDocument LoadDocument( IEnumerable<NavigatorUrl> urls )
        {
            return myDocumentBrowser.LoadDocument( urls );
        }

        public event Action<Uri> Navigating;

        public event Action<System.Windows.Forms.HtmlDocument> DocumentCompleted;
    }
}
