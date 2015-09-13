using System;
using System.Threading;
using System.Windows.Forms;
using RaynMaker.Import.Parsers.Html.WinForms;

namespace RaynMaker.Import.Documents
{
    // TODO: at the moment we dont do any cleanup!
    class WinFormHtmlDocumentLoader : IDocumentLoader, IDisposable
    {
        private WebBrowser myBrowser;
        private bool myOwnWebBrowser = false;

        public WinFormHtmlDocumentLoader( )
            : this( new WebBrowser() )
        {
            // always control the download settings
            DocumentLoaderFactory.CreateDownloadController().HookUp( myBrowser );
         
            myOwnWebBrowser = true;
        }

        public WinFormHtmlDocumentLoader(  WebBrowser webBrowser )
        {
            myBrowser = webBrowser;

            // download settings controlled by DocumentProcessorsFactory
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

            return new HtmlDocumentHandle( new HtmlDocumentAdapter( myBrowser.Document ) );
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
