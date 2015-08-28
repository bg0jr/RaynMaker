using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Blade;
using RaynMaker.Import.Html;
using RaynMaker.Import.Html.WinForms;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Core
{
    public class LegacyDocumentBrowser : ManagedObject, IServiceComponent, IDocumentBrowser
    {
        private DownloadController myDownloadController;
        private bool myIsInitialized = false;
        private bool myOwnWebBrowser = false;
        private const int DownloadChunkSize = 1024; // bytes

        public LegacyDocumentBrowser()
            : this( new WebBrowser() )
        {
            myOwnWebBrowser = true;
        }

        /// <summary>
        /// Creates a WebScrapSC with a given/existing WebBrowser control.
        /// The ownership of the WebBrowser control passed is not taken over.
        /// </summary>
        public LegacyDocumentBrowser( WebBrowser webBrowser )
        {
            myOwnWebBrowser = false;

            Browser = webBrowser;
            Browser.Navigating += WebBrowser_Navigating;

            myDownloadController = new DownloadController();
            myDownloadController.Options = BrowserOptions.NotRunActiveX | BrowserOptions.NoActiveXDownload |
                BrowserOptions.NoBehaviors | BrowserOptions.NoJava | BrowserOptions.NoScripts |
                BrowserOptions.Utf8;
        }

        protected override void Dispose( bool disposing )
        {
            try
            {
                if ( IsDisposed )
                {
                    return;
                }

                if ( disposing )
                {
                    // TODO: restore the old proxy
                    if ( myOwnWebBrowser )
                    {
                        Browser.Dispose();
                    }
                }

                Browser = null;
            }
            finally
            {
                base.Dispose( disposing );
            }
        }

        public void Init( ServiceProvider serviceProvider )
        {
        }

        public WebBrowser Browser
        {
            get;
            private set;
        }

        public HtmlDocument Document
        {
            get { return Browser.Document; }
        }

        /// <summary>
        /// Default options: 
        ///     BrowserOptions.DontRunActiveX | BrowserOptions.NoActiveXDownload |
        ///     BrowserOptions.NoBehaviours | BrowserOptions.NoJava | BrowserOptions.NoScripts |
        ///     BrowserOptions.UTF8
        /// </summary>
        public IDownloadController DownloadController
        {
            get { return myDownloadController; }
        }

        public void Navigate( string url )
        {
            Browser.Navigate( url );
        }

        /// <summary>
        /// Loads a document specified by the given user steps.
        /// Regular expressions in response URLs (embedded in {}) are matched. The
        /// resulting parameter is set to the next request URL using 
        /// string.Format() at placeholder {0}.
        /// </summary>
        public IHtmlDocument LoadDocument( IEnumerable<NavigatorUrl> navigationSteps )
        {
            string url = NavigateToFinalSite( navigationSteps );
            return LoadDocument( url );
        }

        /// <summary>
        /// Navigates to the final site specified by the user steps and
        /// returns the Uri of the last site.
        /// </summary>
        private string NavigateToFinalSite( IEnumerable<NavigatorUrl> navigationSteps )
        {
            IHtmlDocument doc = null;
            string param = null;

            var last = navigationSteps.Last().UrlString;
            foreach ( NavigatorUrl navUrl in navigationSteps )
            {
                this.Require( x => navUrl.UrlType != UriType.None );

                if ( navUrl.UrlType == UriType.Request )
                {
                    string url = navUrl.UrlString;
                    if ( param != null )
                    {
                        url = string.Format( url, param );
                    }
                    else if ( HasPlaceHolder( url ) )
                    {
                        var ex = new ApplicationException( "Did not find a parameter for placeholder" );
                        ex.Data[ "Url" ] = url;

                        throw ex;
                    }

                    if ( navUrl.UrlString == last )
                    {
                        return url;
                    }
                    else
                    {
                        doc = LoadDocument( url );
                    }
                }
                else
                {
                    // get param from response url if any
                    param = PatternMatching.MatchEmbeddedRegex( navUrl.UrlString, doc.Url.ToString() );
                }
            }

            // list empty?
            return null;
        }

        private IHtmlDocument LoadDocument( string url )
        {
            Browser.Navigate( url );

            while ( !
                (Browser.ReadyState == WebBrowserReadyState.Complete ||
                (Browser.ReadyState == WebBrowserReadyState.Interactive && !Browser.IsBusy)) )
            {
                Thread.Sleep( 100 );
                Application.DoEvents();
            }

            return new HtmlDocumentAdapter(Browser.Document);
        }
        
        public IDocument GetDocument( Navigation navi )
        {
            var doc = TryNavigateWithWildcards( navi );
            if ( doc != null )
            {
                return doc;
            }

            if ( navi.DocumentType == DocumentType.Html )
            {
                return new HtmlDocumentHandle( LoadDocument( navi.Uris ) );
            }
            else if ( navi.DocumentType == DocumentType.Text )
            {
                return new TextDocument( DownloadFile( navi.Uris ) );
            }

            throw new NotSupportedException( "DocumentType: " + navi.DocumentType );
        }

        /// <summary>
        /// Downloads a file specified by the given user steps.
        /// </summary>
        private string DownloadFile( IEnumerable<NavigatorUrl> navigationSteps )
        {
            string url = NavigateToFinalSite( navigationSteps );
            return DownloadFile( new Uri(url) );
        }
        
        private string DownloadFile( string uri )
        {
            // TODO: check for protocol in url - maybe the file is local

            return DownloadFile( new Uri( uri ) );
        }

        private string DownloadFile( Uri uri )
        {
            if ( uri.IsFile )
            {
                return ( File.Exists( uri.LocalPath ) ? uri.LocalPath : null );
            }

            WebRequest request = WebRequest.Create( uri );

            HttpWebResponse response = null;
            Stream responseStream = null;
            try
            {
                // Send the 'HttpWebRequest' and wait for response.
                response = (HttpWebResponse)request.GetResponse();
                responseStream = response.GetResponseStream();

                //System.Text.Encoding ec = System.Text.Encoding.GetEncoding( "utf-8" );
                string file = Path.GetTempFileName();
                using ( StreamReader reader = new StreamReader( responseStream ) )
                {
                    char[] chars = new Char[ DownloadChunkSize ];
                    int count = reader.Read( chars, 0, DownloadChunkSize );

                    using ( StreamWriter writer = new StreamWriter( file ) )
                    {
                        while ( count > 0 )
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
                if ( response != null )
                {
                    response.Close();
                }
                if ( responseStream != null )
                {
                    responseStream.Close();
                }
            }
        }

        private IDocument Navigate( DocumentType docType, NavigatorUrl url )
        {
            if ( docType == DocumentType.Html )
            {
                return new HtmlDocumentHandle( LoadDocument( url.UrlString ) );
            }
            else if ( docType == DocumentType.Text )
            {
                return new TextDocument( DownloadFile( url.UrlString ) );
            }

            throw new NotSupportedException( "DocumentType: " + docType );
        }

        // TODO: this should not only happen when loading a document with a complete
        // navigation -> policy based?
        private IDocument TryNavigateWithWildcards( Navigation navi )
        {
            if ( navi.Uris.Count != 1 )
            {
                // we can only handle single urls
                return null;
            }

            var url = navi.Uris[ 0 ];
            Uri uri = new Uri( url.UrlString );
            if ( !uri.IsFile && !uri.IsUnc )
            {
                // we cannot handle e.g. http now
                return null;
            }

            // currently we only handle "/xyz/*/file.txt"
            int pos = url.UrlString.IndexOf( "/*/" );
            if ( pos <= 0 )
            {
                // no pattern found
                return null;
            }

            string root = url.UrlString.Substring( 0, pos );
            string file = url.UrlString.Substring( pos + 3 );
            string[] dirs = Directory.GetDirectories( root, "*" );

            // now try everything with "or" 
            // first path which returns s.th. wins
            foreach ( string dir in dirs )
            {
                string tmpFile = Path.Combine( dir, file );
                if ( !File.Exists( tmpFile ) )
                {
                    continue;
                }

                var doc = Navigate( navi.DocumentType, new NavigatorUrl( url.UrlType, tmpFile ) );
                if ( doc != null )
                {
                    return doc;
                }
            }

            // so in this case we got a pattern navigation url but we were not able
            // to navigate to that url --> throw an exception
            throw new Exception( "Failed to navigate to the document" );
        }

        private bool HasPlaceHolder( string url )
        {
            int begin = url.IndexOf( '{' );
            int end = url.IndexOf( '}' );

            if ( begin < 0 || end < 0 )
            {
                return false;
            }

            return Math.Abs( end - begin ) < 3;
        }

        private void WebBrowser_Navigating( object sender, WebBrowserNavigatingEventArgs e )
        {
            if ( IsDisposed )
            {
                return;
            }

            if ( myIsInitialized )
            {
                return;
            }

            myDownloadController.HookUp( Browser );

            myIsInitialized = true;
        }

        private Uri SelectUrl( IList<string> urls )
        {
            int idx = 0;

            string url = urls[ idx ];
            urls.RemoveAt( idx );

            return new Uri( url );
        }
    }
}
