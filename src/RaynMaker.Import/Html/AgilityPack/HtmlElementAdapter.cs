using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace RaynMaker.Import.Html.AgilityPack
{
    [CLSCompliant( false )]
    public class HtmlElementAdapter : IHtmlElement
    {
        internal HtmlElementAdapter( HtmlDocumentAdapter document, HtmlNode element )
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

        public HtmlNode Element
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
            get { return Element.ParentNode == null ? null : DocumentAdapter.Create( Element.ParentNode ); }
        }

        public IEnumerable<IHtmlElement> Children
        {
            get
            {
                return Element.ChildNodes
                    .Select( e => DocumentAdapter.Create( e ) )
                    .ToList();
            }
        }

        public string TagName
        {
            get { return Element.Name; }
        }

        public string GetAttribute( string attr )
        {
            return Element.Attributes[ attr ].Value;
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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append( TagName );

            return sb.ToString();
        }
    }
}
