using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaynMaker.Import.Core;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import
{
    public class DocumentLoaderFactory
    {
        public static IDocumentLoader Create( DocumentType docType )
        {
            if ( docType == DocumentType.Html )
            {
                // this loader is somehow heavyweight - BUT we still want to create always a new instance as we can only support
                // multiple instances of html documents if we have multiple instances of this loader (because web browser is used behind)
                // and this behaviour is more intuitive
                return new WinFormHtmlDocumentLoader();
            }
            else if ( docType == DocumentType.Text )
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
