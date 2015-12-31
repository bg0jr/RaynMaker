using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace RaynMaker.Import.Documents.AgilityPack
{
    public class HtmlDocumentAdapter : IHtmlDocument
    {
        private IDictionary<HtmlNode, HtmlElementAdapter> myElementAdapters;

        public HtmlDocumentAdapter( Uri uri, HtmlDocument doc )
        {
            if( doc == null )
            {
                throw new ArgumentNullException( "doc" );
            }

            Location = uri;
            Document = doc;
            myElementAdapters = new Dictionary<HtmlNode, HtmlElementAdapter>();
        }

        public Uri Location { get; private set; }

        public HtmlDocument Document { get; private set; }

        public IHtmlElement Body
        {
            get { return Create( Document.DocumentNode.SelectSingleNode( "//body" ) ); }
        }

        public HtmlElementAdapter Create( HtmlNode element )
        {
            if( !myElementAdapters.ContainsKey( element ) )
            {
                myElementAdapters[ element ] = new HtmlElementAdapter( this, element );
            }

            return myElementAdapters[ element ];
        }
    }
}
