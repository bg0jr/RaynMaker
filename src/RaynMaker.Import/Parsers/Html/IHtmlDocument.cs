using System;

namespace RaynMaker.Import.Parsers.Html
{
    public interface IHtmlDocument
    {
        Uri Url { get; }

        IHtmlElement Body { get; }
    }
}
