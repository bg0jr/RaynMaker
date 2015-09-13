using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using RaynMaker.Import.Parsers.Html;
using RaynMaker.Import.Parsers.Html.WinForms;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Documents
{
    class WinFormsDocumentBrowser : IDisposable, IDocumentBrowser
    {
        private DownloadController myDownloadController;
        private bool myIsInitialized = false;
        private bool myOwnWebBrowser = false;
        private const int DownloadChunkSize = 1024; // bytes
        private INavigator myNavigator;

        public WinFormsDocumentBrowser( INavigator navigator )
            : this( navigator, new WebBrowser() )
        {
            myOwnWebBrowser = true;
        }

        public WinFormsDocumentBrowser( INavigator navigator, WebBrowser webBrowser )
        {
            myNavigator = navigator;

            Browser = webBrowser;
            Browser.Navigating += WebBrowser_Navigating;
            Browser.DocumentCompleted += WebBrowser_DocumentCompleted;

            myOwnWebBrowser = false;

            myDownloadController = new DownloadController();
        }

        public void Dispose()
        {
            if( Browser != null )
            {
                Browser.Navigating -= WebBrowser_Navigating;
                Browser.DocumentCompleted -= WebBrowser_DocumentCompleted;

                if( myOwnWebBrowser )
                {
                    Browser.Dispose();
                }

                Browser = null;
            }
        }

        public WebBrowser Browser { get; private set; }

        public IDownloadController DownloadController
        {
            get { return myDownloadController; }
        }

        public IDocument Document { get; private set; }

        public void Navigate( DocumentType docType, Uri url )
        {
            if( docType == DocumentType.Html )
            {
                Document = new HtmlDocumentHandle( LoadDocument( url ) );
            }
            else if( docType == DocumentType.Text )
            {
                Document = new TextDocument( DownloadFile( url ) );
            }
            else
            {
                throw new NotSupportedException( "DocumentType: " + docType );
            }
        }

        public void Navigate( Navigation navi )
        {
            var url = myNavigator.Navigate( navi );
            if( url == null )
            {
                return;
            }

            if( navi.DocumentType == DocumentType.Html )
            {
                Document = new HtmlDocumentHandle( LoadDocument( url ) );
            }
            else if( navi.DocumentType == DocumentType.Text )
            {
                Document = new TextDocument( DownloadFile( url ) );
            }
            else
            {
                throw new NotSupportedException( "DocumentType: " + navi.DocumentType );
            }
        }

        private IHtmlDocument LoadDocument( Uri url )
        {
            Browser.Navigate( url );

            while( !
                ( Browser.ReadyState == WebBrowserReadyState.Complete ||
                ( Browser.ReadyState == WebBrowserReadyState.Interactive && !Browser.IsBusy ) ) )
            {
                Thread.Sleep( 100 );
                Application.DoEvents();
            }

            return new HtmlDocumentAdapter( Browser.Document );
        }

        private string DownloadFile( Uri uri )
        {
            if( uri.IsFile )
            {
                return ( File.Exists( uri.LocalPath ) ? uri.LocalPath : null );
            }

            WebRequest request = WebRequest.Create( uri );

            HttpWebResponse response = null;
            Stream responseStream = null;
            try
            {
                // Send the 'HttpWebRequest' and wait for response.
                response = ( HttpWebResponse )request.GetResponse();
                responseStream = response.GetResponseStream();

                //System.Text.Encoding ec = System.Text.Encoding.GetEncoding( "utf-8" );
                string file = Path.GetTempFileName();
                using( StreamReader reader = new StreamReader( responseStream ) )
                {
                    char[] chars = new Char[ DownloadChunkSize ];
                    int count = reader.Read( chars, 0, DownloadChunkSize );

                    using( StreamWriter writer = new StreamWriter( file ) )
                    {
                        while( count > 0 )
                        {
                            writer.Write( chars, 0, count );
                            count = reader.Read( chars, 0, DownloadChunkSize );
                        }
                    }
                }

                // TODO: who removes this temp file?
                return file;
            }
            finally
            {
                if( response != null )
                {
                    response.Close();
                }
                if( responseStream != null )
                {
                    responseStream.Close();
                }
            }
        }

        private void WebBrowser_Navigating( object sender, WebBrowserNavigatingEventArgs e )
        {
            if( Browser == null )
            {
                // already disposed
                return;
            }

            if( myIsInitialized )
            {
                return;
            }

            myDownloadController.HookUp( Browser );

            myIsInitialized = true;

            if( Navigating != null )
            {
                Navigating( e.Url );
            }
        }

        private void WebBrowser_DocumentCompleted( object sender, WebBrowserDocumentCompletedEventArgs e )
        {
            Document = new HtmlDocumentHandle( new HtmlDocumentAdapter( Browser.Document ) );

            if( DocumentCompleted != null )
            {
                DocumentCompleted( Document );
            }
        }

        public event Action<Uri> Navigating;

        public event Action<IDocument> DocumentCompleted;
    }
}
