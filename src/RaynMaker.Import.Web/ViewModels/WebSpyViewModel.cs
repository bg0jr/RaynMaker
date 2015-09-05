using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Import.Core;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Web.ViewModels
{
    [Export]
    class WebSpyViewModel : BindableBase, IBrowser
    {
        private LegacyDocumentBrowser myDocumentBrowser = null;

        public WebSpyViewModel()
        {
            AddressBar = new AddressBarViewModel();
            Datums = new DatumSelectionViewModel();
            Navigation = new NavigationViewModel();
            DataFormat = new DataFormatViewModel();
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

                AddressBar.Browser = this;
                Navigation.Browser = this;

                Navigate( "http://www.google.com" );
            }
        }

        private void myBrowser_Navigating( object sender, System.Windows.Forms.WebBrowserNavigatingEventArgs e )
        {
            if( Navigation.IsCapturing )
            {
                Navigation.NavigationUrls += new NavigatorUrl( UriType.Request, e.Url ).ToString();
                Navigation.NavigationUrls += Environment.NewLine;
            }
        }

        private void myBrowser_DocumentCompleted( object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e )
        {
            DataFormat.Document = myDocumentBrowser.Document;

            AddressBar.Url = myDocumentBrowser.Document.Url.ToString();

            if( Navigation.IsCapturing )
            {
                Navigation.NavigationUrls += new NavigatorUrl( UriType.Response, myDocumentBrowser.Browser.Document.Url ).ToString();
                Navigation.NavigationUrls += Environment.NewLine;
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

        public void Navigate( string url )
        {
            myDocumentBrowser.Navigate( url );
        }

        public void LoadDocument( IEnumerable<NavigatorUrl> urls )
        {
            myDocumentBrowser.LoadDocument( urls );
        }

        public AddressBarViewModel AddressBar { get; private set; }

        public DatumSelectionViewModel Datums { get; private set; }

        public NavigationViewModel Navigation { get; private set; }

        public DataFormatViewModel DataFormat { get; private set; }
    }
}
