using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Plainion;

namespace RaynMaker.Modules.Import.Documents.WinForms
{
    public class HtmlElementAdapter : IHtmlElement
    {
        internal HtmlElementAdapter( HtmlDocumentAdapter document, HtmlElement element )
        {
            Contract.RequiresNotNull( document, "document" );
            Contract.RequiresNotNull( element, "element" );

            DocumentAdapter = document;
            Element = element;
        }

        public HtmlDocumentAdapter DocumentAdapter { get; private set; }

        public HtmlElement Element { get; private set; }

        public IHtmlDocument Document
        {
            get { return DocumentAdapter; }
        }

        public IHtmlElement Parent
        {
            get { return Element.Parent == null ? null : DocumentAdapter.Create( Element.Parent ); }
        }

        public IEnumerable<IHtmlElement> Children
        {
            get
            {
                return Element.Children
                    .OfType<HtmlElement>()
                    .Select( e => DocumentAdapter.Create( e ) )
                    .ToList();
            }
        }

        public string TagName
        {
            get { return Element.TagName; }
        }

        public string GetAttribute( string attr )
        {
            return Element.GetAttribute( attr );
        }

        public string InnerText
        {
            get { return Element.InnerText; }
        }

        public string InnerHtml
        {
            get { return Element.InnerHtml; }
        }


        public string Style
        {
            get { return Element.Style; }
            set { Element.Style = value; }
        }
    }
}
