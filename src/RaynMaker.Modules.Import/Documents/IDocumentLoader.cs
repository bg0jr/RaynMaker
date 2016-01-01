using System;

namespace RaynMaker.Modules.Import.Documents
{
    interface IDocumentLoader
    {
        IDocument Load( Uri uri );
    }
}
