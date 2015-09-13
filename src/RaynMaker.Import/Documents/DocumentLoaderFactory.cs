using System;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Documents
{
    class DocumentLoaderFactory
    {
        public static IDocumentLoader CreateLoader( DocumentType docType, System.Windows.Forms.WebBrowser webBrowser )
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

        public static DownloadController CreateDownloadController()
        {
            return new DownloadController
            {
                Options = BrowserOptions.NotRunActiveX | BrowserOptions.NoActiveXDownload |
                    BrowserOptions.NoBehaviors | BrowserOptions.NoJava | BrowserOptions.NoScripts |
                    BrowserOptions.Utf8
            };
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

    }
}
