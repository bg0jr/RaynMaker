using System;
using RaynMaker.Import.Documents;

namespace RaynMaker.Import.Documents
{
    class TextDocumentLoader : IDocumentLoader
    {
        public IDocument Load( Uri uri )
        {
            var localFile = WebUtil.Download( uri );

            return new TextDocument( localFile );
        }
    }
}
