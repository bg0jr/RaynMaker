using System;
using RaynMaker.Modules.Import.Documents;

namespace RaynMaker.Modules.Import.Documents
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
