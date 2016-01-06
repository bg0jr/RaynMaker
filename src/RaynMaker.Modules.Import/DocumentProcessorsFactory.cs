using System;
using RaynMaker.Modules.Import.Design;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Parsers.Html;
using RaynMaker.Modules.Import.Parsers.Text;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import
{
    public static class DocumentProcessorsFactory
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
            return new WinFormsDocumentBrowser( CreateNavigator(), webBrowser );
        }

        public static IDocumentParser CreateParser( IDocument document, IFigureDescriptor descriptor )
        {
            var htmlDocument = document as IHtmlDocument;
            if( htmlDocument != null )
            {
                return new HtmlParser( htmlDocument, descriptor );
            }

            var textDocument = document as TextDocument;
            if( textDocument != null )
            {
                return new TextParser( textDocument, descriptor );
            }

            throw new NotSupportedException( "Unable to find parser for document type: " + document.GetType() );
        }
    }
}
