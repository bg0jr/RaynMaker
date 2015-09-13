using System;
using System.Threading;
using System.Windows.Forms;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Parsers.Html.WinForms;

namespace RaynMaker.Import.Core
{
    // TODO: at the moment we dont do any cleanup!
    class WinFormHtmlDocumentLoader : IDocumentLoader, IDisposable
    {
        private WebBrowser myBrowser;
        private DownloadController myDownloadController;
        private bool myIsInitialized = false;

        public WinFormHtmlDocumentLoader()
        {
            myBrowser = new WebBrowser();
            myBrowser.Navigating += WebBrowser_Navigating;

            myDownloadController = new DownloadController();
            myDownloadController.Options = BrowserOptions.NotRunActiveX | BrowserOptions.NoActiveXDownload |
                BrowserOptions.NoBehaviors | BrowserOptions.NoJava | BrowserOptions.NoScripts |
                BrowserOptions.Utf8;
        }

        private void WebBrowser_Navigating( object sender, WebBrowserNavigatingEventArgs e )
        {
            if ( myIsInitialized )
            {
                return;
            }

            myDownloadController.HookUp( myBrowser );

            myIsInitialized = true;
        }


        public IDocument Load( Uri uri )
        {
            return new HtmlDocumentHandle( new HtmlDocumentAdapter( LoadHtmlDocument( uri ) ) );
        }

        public HtmlDocument LoadHtmlDocument( Uri url )
        {
            myBrowser.Navigate( url );

            while ( !
                (myBrowser.ReadyState == WebBrowserReadyState.Complete ||
                (myBrowser.ReadyState == WebBrowserReadyState.Interactive && !myBrowser.IsBusy)) )
            {
                Thread.Sleep( 100 );
                Application.DoEvents();
            }

            return myBrowser.Document;
        }

        public void Dispose()
        {
            if ( myBrowser == null )
            {
                return;
            }

            myBrowser.Navigating -= WebBrowser_Navigating;
            myBrowser.Dispose();
            myBrowser = null;
        }
    }
}
