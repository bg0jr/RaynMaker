using System;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import
{
    /// <summary>
    /// Collects convenience methods for using IDocumentBrowser while keeping actual IDocumentBrowser interface slim.
    /// </summary>
    public static class DocumentBrowserExtensions
    {
        public static void Navigate( this IDocumentBrowser self, DocumentType docType, Uri url )
        {
            self.Navigate( docType, new DocumentLocator( new Request( url ) ) );
        }

        public static void Navigate( this IDocumentBrowser self, DocumentType docType, DocumentLocator locator )
        {
            self.Navigate( docType, locator, new NullLocatorMacroResolver() );
        }

        public static T LoadDocument<T>( this IDocumentBrowser self, string url ) where T : IDocument
        {
            var docType = GetDocumentType( typeof( T ) );

            self.Navigate( docType, new Uri( url ) );

            return ( T )self.Document;
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
