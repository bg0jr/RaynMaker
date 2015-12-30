using System;
using System.Collections.Generic;
using System.Linq;

using WinFormsHtmlElement = System.Windows.Forms.HtmlElement;

namespace RaynMaker.Import.Documents.WinForms
{
    public class HtmlElement : IHtmlElement
    {
        internal HtmlElement( HtmlDocument document, WinFormsHtmlElement element )
        {
            if( document == null )
            {
                throw new ArgumentNullException( "document" );
            }
            if( element == null )
            {
                throw new ArgumentNullException( "element" );
            }

            DocumentAdapter = document;
            Element = element;
        }

        public HtmlDocument DocumentAdapter { get; private set; }

        public WinFormsHtmlElement Element { get; private set; }

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
                    .OfType<WinFormsHtmlElement>()
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
    }
}
