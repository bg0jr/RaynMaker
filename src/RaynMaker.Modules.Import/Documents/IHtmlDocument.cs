using System;

namespace RaynMaker.Modules.Import.Documents
{
    public interface IHtmlDocument : IDocument
    {
        IHtmlElement Body { get; }

        IHtmlElement GetElementById( string id );
    }
}
