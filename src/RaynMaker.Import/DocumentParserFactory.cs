using System;
using System.Data;
using RaynMaker.Import.Core;
using RaynMaker.Import.Html;
using RaynMaker.Import.Parsers;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import
{
    public class DocumentParserFactory
    {
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
