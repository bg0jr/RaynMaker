using System;

namespace RaynMaker.Import.Html
{
    public interface IHtmlDocument
    {
        Uri Url { get; }

        IHtmlElement Body { get; }
    }
}
