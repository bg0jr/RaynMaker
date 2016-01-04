using System.Drawing;
using Plainion;
using RaynMaker.Modules.Import.Documents;

namespace RaynMaker.Modules.Import.Design
{
    public class HtmlElementMarker : IHtmlMarker
    {
        public static string MarkupClass = "__rym_markup__";

        private string myOriginalStyle;

        public HtmlElementMarker( Color color )
        {
            Color = color;
        }

        public Color Color { get; private set; }

        public IHtmlElement Element { get; private set; }

        public void Mark( IHtmlElement element )
        {
            Contract.RequiresNotNull( element != null, "element" );

            if( Element != null )
            {
                // unmark first - maybe it was marked with another color before
                Unmark();
            }

            Element = element;

            myOriginalStyle =Element.Style;
            Element.Style+= string.Format( ";color:black;background-color:{0}", ColorTranslator.ToHtml( Color ) );
        }

        public void Unmark()
        {
            if( Element == null )
            {
                return;
            }

            Element.Style = myOriginalStyle;

            Element = null;
            myOriginalStyle = null;
        }

        public void Reset()
        {
            Unmark();
        }
    }
}
