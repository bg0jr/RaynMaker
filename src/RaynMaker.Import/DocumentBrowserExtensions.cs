using System;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Spec.v2;

namespace RaynMaker.Import
{
    public static class DocumentBrowserExtensions
    {
        public static T LoadDocument<T>( this IDocumentBrowser browser, string url ) where T : IDocument
        {
            var docType = GetDocumentType( typeof( T ) );

            browser.Navigate( docType, new Uri( url ) );

            return ( T )browser.Document;
        }

        private static DocumentType GetDocumentType( Type type )
        {
            if( type == typeof( IHtmlDocument ) )
            {
                return DocumentType.Html;
            }
            else if( type == typeof( TextDocument ) )
            {
                return DocumentType.Text;
            }
            else
            {
                throw new NotSupportedException( "Unknown document type: " + type );
            }
        }
    }
}
