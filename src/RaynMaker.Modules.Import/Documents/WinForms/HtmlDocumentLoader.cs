using System;
using System.Threading;
using System.Windows.Forms;
using RaynMaker.Modules.Import.Design;

namespace RaynMaker.Modules.Import.Documents.WinForms
{
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
            return myBrowser.Load( uri );
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
