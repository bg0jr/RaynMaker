using System;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Parsers.Html;
using RaynMaker.Import.Parsers.Text;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import
{
    public class DocumentProcessorsFactory
    {
        public static IDocumentBrowser CreateBrowser()
        {
            return CreateEnhancedDocumentBrowser();
        }

        private static IDocumentBrowser CreateLegacyDocumentBrowser()
        {
            var browser = new WinFormsDocumentBrowser();

            browser.DownloadController.Options = BrowserOptions.NoActiveXDownload |
                BrowserOptions.NoBehaviors | BrowserOptions.NoJava | BrowserOptions.NoScripts |
                BrowserOptions.Utf8;

            return browser;
        }

        private static IDocumentBrowser CreateEnhancedDocumentBrowser()
        {
            var navigator = new CachingNavigator(
                new Navigator(),
                new DocumentCache() );

            var browser = new DocumentBrowser( navigator );

            return browser;
        }

        public static IDocumentLoader CreateLoader( DocumentType docType )
        {
            if( docType == DocumentType.Html )
            {
                // this loader is somehow heavyweight - BUT we still want to create always a new instance as we can only support
                // multiple instances of html documents if we have multiple instances of this loader (because web browser is used behind)
                // and this behaviour is more intuitive
                return new WinFormHtmlDocumentLoader();
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

        public static IDocumentParser CreateParser( IDocument document, IFormat format )
        {
            var htmlDocument = document as HtmlDocumentHandle;
            if( htmlDocument != null )
            {
                return new HtmlParser( htmlDocument, format );
            }

            var textDocument = document as TextDocument;
            if( textDocument != null )
            {
                return new TextParser( textDocument, format );
            }

            throw new NotSupportedException( "Unable to find parser for document type: " + document.GetType() );
        }
    }
}
