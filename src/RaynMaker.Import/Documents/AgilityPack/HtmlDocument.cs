using System;
using System.Collections.Generic;
using HtmlAgilityPack;

using AgilityPackHtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace RaynMaker.Import.Documents.AgilityPack
{
    public class HtmlDocument : IHtmlDocument
    {
        private IDictionary<HtmlNode, HtmlElement> myElementAdapters;

        public HtmlDocument( Uri uri, AgilityPackHtmlDocument doc )
        {
            if( doc == null )
            {
                throw new ArgumentNullException( "doc" );
            }

            Location = uri;
            Document = doc;
            myElementAdapters = new Dictionary<HtmlNode, HtmlElement>();
        }

        public Uri Location { get; private set; }

        public AgilityPackHtmlDocument Document { get; private set; }

        public IHtmlElement Body
        {
            get { return Create( Document.DocumentNode.SelectSingleNode( "//body" ) ); }
        }

        public HtmlElement Create( HtmlNode element )
        {
            if( !myElementAdapters.ContainsKey( element ) )
            {
                myElementAdapters[ element ] = new HtmlElement( this, element );
            }

            return myElementAdapters[ element ];
        }
    }
}
