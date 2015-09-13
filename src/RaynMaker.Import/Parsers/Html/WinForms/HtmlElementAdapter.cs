using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RaynMaker.Import.Parsers.Html.WinForms
{
    public class HtmlElementAdapter : IHtmlElement
    {
        internal HtmlElementAdapter( HtmlDocumentAdapter document, HtmlElement element )
        {
            if ( document == null )
            {
                throw new ArgumentNullException( "document" );
            }
            if ( element == null )
            {
                throw new ArgumentNullException( "element" );
            }

            DocumentAdapter = document;
            Element = element;
        }

        public HtmlDocumentAdapter DocumentAdapter
        {
            get;
            private set;
        }

        public HtmlElement Element
        {
            get;
            private set;
        }

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
            get
            {
                return Element.InnerText;
            }
        }

        public string InnerHtml
        {
            get
            {
                return Element.InnerHtml;
            }
        }
    }
}
