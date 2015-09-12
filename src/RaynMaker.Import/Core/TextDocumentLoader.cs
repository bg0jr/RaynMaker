using System;

namespace RaynMaker.Import.Core
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
