using System;

namespace RaynMaker.Import.Documents
{
    interface IDocumentLoader
    {
        IDocument Load( Uri uri );
    }
}
