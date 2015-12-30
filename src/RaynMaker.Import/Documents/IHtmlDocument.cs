using System;

namespace RaynMaker.Import.Documents
{
    public interface IHtmlDocument : IDocument
    {
        IHtmlElement Body { get; }
    }
}
