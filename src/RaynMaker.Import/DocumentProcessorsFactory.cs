using System;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Parsers.Html;
using RaynMaker.Import.Parsers.Text;
using RaynMaker.Import.Spec;
using RaynMaker.Import.WinForms;

namespace RaynMaker.Import
{
    public class DocumentProcessorsFactory
    {
        public static IDocumentBrowser CreateBrowser()
        {
            return new DocumentBrowser( CreateNavigator() );
        }

        private static CachingNavigator CreateNavigator()
        {
            var navigator = new CachingNavigator(
                new Navigator(),
                new DocumentCache() );
            return navigator;
        }

        public static IDocumentBrowser CreateBrowser( SafeWebBrowser webBrowser )
        {
            // always control the download settings
            webBrowser.DownloadControlFlags = DocumentLoaderFactory.DownloadControlFlags;

            return new WinFormsDocumentBrowser( new Navigator(), webBrowser );
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
