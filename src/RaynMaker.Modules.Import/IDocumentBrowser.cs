using System;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import
{
    public interface IDocumentBrowser
    {
        IDocument Document { get; }

        void Navigate( DocumentType docType, DocumentLocator locator, ILocatorMacroResolver macroResolver );

        event Action<Uri> Navigating;

        event Action<IDocument> DocumentCompleted;

        void ClearCache();
    }
}
