using System;
using System.Data;
using RaynMaker.Import.Parsers.Html;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Documents
{
    public class HtmlDocumentHandle : IDocument
    {
        public HtmlDocumentHandle( IHtmlDocument doc )
        {
            Content = doc;
            Location = doc.Url;
        }

        public Uri Location { get; private set; }

        public IHtmlDocument Content { get; private set; }
    }
}
