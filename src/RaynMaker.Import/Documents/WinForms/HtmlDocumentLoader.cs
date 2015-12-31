using System;
using System.Threading;
using System.Windows.Forms;
using RaynMaker.Import.Documents.WinForms;
using RaynMaker.Import.WinForms;

namespace RaynMaker.Import.Documents.WinForms
{
    // TODO: at the moment we dont do any cleanup!
    class HtmlDocumentLoader : IDocumentLoader, IDisposable
    {
        private SafeWebBrowser myBrowser;
        private bool myOwnWebBrowser = false;

        public HtmlDocumentLoader()
            : this( new SafeWebBrowser() )
        {
            myOwnWebBrowser = true;
        }

        public HtmlDocumentLoader( SafeWebBrowser webBrowser )
        {
            myBrowser = webBrowser;

            // always control the download settings
            webBrowser.DownloadControlFlags = DocumentLoaderFactory.DownloadControlFlags;
        }

        public IDocument Load( Uri uri )
        {
            myBrowser.Navigate( uri );

            while( !
                ( myBrowser.ReadyState == WebBrowserReadyState.Complete ||
                ( myBrowser.ReadyState == WebBrowserReadyState.Interactive && !myBrowser.IsBusy ) ) )
            {
                Thread.Sleep( 100 );
                Application.DoEvents();
            }

            return new HtmlDocumentAdapter( myBrowser.Document );
        }

        public void Dispose()
        {
            if( myBrowser == null )
            {
                return;
            }

            if( myOwnWebBrowser )
            {
                myBrowser.Dispose();
            }
            myBrowser = null;
        }
    }
}
