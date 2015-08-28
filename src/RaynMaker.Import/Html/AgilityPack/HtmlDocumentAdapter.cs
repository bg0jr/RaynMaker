using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace RaynMaker.Import.Html.AgilityPack
{
    //[CLSCompliant(false)]
    public class HtmlDocumentAdapter : IHtmlDocument
    {
        private IDictionary<HtmlNode, HtmlElementAdapter> myElementAdapters;

        public HtmlDocumentAdapter( Uri uri, HtmlDocument doc )
        {
            if ( doc == null )
            {
                throw new ArgumentNullException( "doc" );
            }

            Url = uri;
            Document = doc;
            myElementAdapters = new Dictionary<HtmlNode, HtmlElementAdapter>();
        }

        public HtmlDocument Document { get; private set; }

        public Uri Url
        {
            get;
            private set;
        }

        public IHtmlElement Body
        {
            get { return Create( Document.DocumentNode.SelectSingleNode( "//body" ) ); }
        }

        public HtmlElementAdapter Create( HtmlNode element )
        {
            if ( !myElementAdapters.ContainsKey( element ) )
            {
                myElementAdapters[ element ] = new HtmlElementAdapter( this, element );
            }

            return myElementAdapters[ element ];
        }
    }
}
