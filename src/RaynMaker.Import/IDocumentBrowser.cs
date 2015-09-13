using System;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import
{
    public interface IDocumentBrowser
    {
        IDocument Document { get; }

        void Navigate( DocumentType docType, Uri url );

        void Navigate( Navigation navi );

        event Action<Uri> Navigating;

        event Action<IDocument> DocumentCompleted;
    }
}
