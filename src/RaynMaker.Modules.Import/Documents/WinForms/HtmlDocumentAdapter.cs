using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RaynMaker.Modules.Import.Documents.WinForms
{
    public class HtmlDocumentAdapter : IHtmlDocument
    {
        private IDictionary<HtmlElement, HtmlElementAdapter> myElementAdapters;

        public HtmlDocumentAdapter( HtmlDocument doc )
        {
            if( doc == null )
            {
                throw new ArgumentNullException( "doc" );
            }

            Document = doc;
            myElementAdapters = new Dictionary<HtmlElement, HtmlElementAdapter>();
        }

        public Uri Location
        {
            get { return Document.Url; }
        }

        public HtmlDocument Document { get; private set; }

        public IHtmlElement Body
        {
            get { return Create( Document.Body ); }
        }

        public HtmlElementAdapter Create( HtmlElement element )
        {
            // does not work :(
            //if ( Document != element.Document )
            //{
            //    throw new InvalidOperationException( "Given element does not belong to this document" );
            //}

            if( !myElementAdapters.ContainsKey( element ) )
            {
                myElementAdapters[ element ] = new HtmlElementAdapter( this, element );
            }

            return myElementAdapters[ element ];
        }

        public IHtmlElement GetElementById( string id )
        {
            return Create( Document.GetElementById( id ) );
        }
    }
}
