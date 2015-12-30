using System;
using System.Collections.Generic;

using WinFormsHtmlDocument = System.Windows.Forms.HtmlDocument;
using WinFormsHtmlElement = System.Windows.Forms.HtmlElement;

namespace RaynMaker.Import.Documents.WinForms
{
    public class HtmlDocument : IHtmlDocument
    {
        private IDictionary<WinFormsHtmlElement, HtmlElement> myElementAdapters;

        public HtmlDocument( WinFormsHtmlDocument doc )
        {
            if( doc == null )
            {
                throw new ArgumentNullException( "doc" );
            }

            Document = doc;
            myElementAdapters = new Dictionary<WinFormsHtmlElement, HtmlElement>();
        }

        public Uri Location
        {
            get { return Document.Url; }
        }

        public WinFormsHtmlDocument Document { get; private set; }

        public IHtmlElement Body
        {
            get { return Create( Document.Body ); }
        }

        public HtmlElement Create( WinFormsHtmlElement element )
        {
            // does not work :(
            //if ( Document != element.Document )
            //{
            //    throw new InvalidOperationException( "Given element does not belong to this document" );
            //}

            if( !myElementAdapters.ContainsKey( element ) )
            {
                myElementAdapters[ element ] = new HtmlElement( this, element );
            }

            return myElementAdapters[ element ];
        }
    }
}
