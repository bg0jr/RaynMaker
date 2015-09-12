using System;

namespace RaynMaker.Import
{
    public interface IDocumentLoader
    {
        IDocument Load( Uri uri );
    }
}
