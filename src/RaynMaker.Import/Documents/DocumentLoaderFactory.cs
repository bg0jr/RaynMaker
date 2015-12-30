using System;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2;
using RaynMaker.Import.Spec.v2.Locating;
using RaynMaker.Import.WinForms;

namespace RaynMaker.Import.Documents
{
    class DocumentLoaderFactory
    {
        public static IDocumentLoader CreateLoader( DocumentType docType, SafeWebBrowser webBrowser )
        {
            if( docType == DocumentType.Html )
            {
                return new WinFormHtmlDocumentLoader(  webBrowser );
            }
            else
            {
                return CreateLoader( docType );
            }
        }

        public static IDocumentLoader CreateLoader( DocumentType docType )
        {
            if( docType == DocumentType.Html )
            {
                // this loader is somehow heavyweight - BUT we still want to create always a new instance as we can only support
                // multiple instances of html documents if we have multiple instances of this loader (because web browser is used behind)
                // and this behaviour is more intuitive
                return new WinFormHtmlDocumentLoader(  );
            }
            else if( docType == DocumentType.Text )
            {
                return new TextDocumentLoader();
            }
            else
            {
                throw new NotSupportedException( "Unknown document type: " + docType );
            }
        }


        public static WebBrowserDownloadControlFlags DownloadControlFlags
        {
            get
            {
                return WebBrowserDownloadControlFlags.NotRunActiveX | WebBrowserDownloadControlFlags.NoActiveXDownload |
                    WebBrowserDownloadControlFlags.NoBehaviors | WebBrowserDownloadControlFlags.NoJava | WebBrowserDownloadControlFlags.NoScripts |
                    WebBrowserDownloadControlFlags.Utf8;
            }
        }
    }
}
