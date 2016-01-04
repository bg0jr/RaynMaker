using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Plainion;
using RaynMaker.Modules.Import.Parsers.Html;

namespace RaynMaker.Modules.Import.Design
{
    class HtmlMarker
    {
        public class MarkedHtmlElement
        {
            public MarkedHtmlElement( HtmlElement value )
            {
                Value = value;
                OriginalStyle = Value.Style;
            }

            public HtmlElement Value { get; private set; }
            public string OriginalStyle { get; private set; }
        }

        private IList<MarkedHtmlElement> myMarkedElements;

        public static Color DefaultColor = Color.Yellow;
        public static string MarkupClass = "__rym_markup__";

        public HtmlMarker()
        {
            myMarkedElements = new List<MarkedHtmlElement>();
        }

        public IEnumerable<MarkedHtmlElement> MarkedElements { get { return myMarkedElements; } }

        public void Mark( HtmlElement element )
        {
            Mark( element, DefaultColor );
        }

        public void Mark( HtmlElement element, Color color )
        {
            Contract.RequiresNotNull( element != null, "element" );
            Contract.RequiresNotNull( color != null, "color" );

            if( IsMarked( element ) )
            {
                // unmark first - maybe it was marked with another color before
                Unmark( element );
            }

            var info = new MarkedHtmlElement( element );

            info.Value.Style += string.Format( ";color:black;background-color:{0}", ColorTranslator.ToHtml( color ) );

            myMarkedElements.Add( info );
        }

        public void Unmark( HtmlElement element )
        {
            Contract.RequiresNotNull( element != null, "element" );

            var markedElement = FindBy( element );

            if( markedElement == null )
            {
                return;
            }

            Unmark( markedElement );
        }

        private void Unmark( MarkedHtmlElement markedElement )
        {
            markedElement.Value.Style = markedElement.OriginalStyle;
            myMarkedElements.Remove( markedElement );
        }

        public void UnmarkAll()
        {
            foreach( var e in myMarkedElements.ToList() )
            {
                Unmark( e );
            }

            Contract.Invariant( myMarkedElements.Count == 0, "No element expected to be selected" );
        }

        public bool IsMarked( HtmlElement element )
        {
            return FindBy( element ) != null;
        }

        public MarkedHtmlElement FindBy( HtmlElement element )
        {
            Contract.RequiresNotNull( element != null, "element" );

            return myMarkedElements.SingleOrDefault( e => e.Value == element );
        }
    }
}
