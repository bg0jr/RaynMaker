using System;
using System.Data;
using RaynMaker.Import.Html;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Core
{
    public class HtmlDocumentHandle : IDocument
    {
        public HtmlDocumentHandle( IHtmlDocument doc )
        {
            Content = doc;
            Location = doc.Url.ToString();
        }

        public string Location { get; private set; }

        public IHtmlDocument Content { get; private set; }
    }
}
