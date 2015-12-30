using System;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2;
using RaynMaker.Import.Spec.v2.Locating;

namespace RaynMaker.Import
{
    public interface IDocumentBrowser
    {
        IDocument Document { get; }

        void Navigate( DocumentType docType, Uri url );

        void Navigate( DocumentType docType, DocumentLocator navi );

        event Action<Uri> Navigating;

        event Action<IDocument> DocumentCompleted;

        void ClearCache();
    }
}
