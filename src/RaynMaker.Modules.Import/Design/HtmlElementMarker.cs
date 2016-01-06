using System.Drawing;
using Plainion;
using RaynMaker.Modules.Import.Documents;

namespace RaynMaker.Modules.Import.Design
{
    /// <summary>
    /// Highlights HTML elements by applying certain background color.
    /// Stacking of <see cref="HtmlElementMarker"/> instances is supported.
    /// </summary>
    public class HtmlElementMarker : IHtmlMarker
    {
        public static string MarkupClass = "__rym_markup__";

        private const string Attribute_OrigStyle = "OrigStyle";
        private const string Attribute_Marker = "Marker";
        private const string Attribute_MarkerCount = "MarkerCount";

        private int myMarkerIdx;

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

            var markerCountAttr = Element.GetAttribute( Attribute_MarkerCount );
            if( string.IsNullOrEmpty( markerCountAttr ) )
            {
                Element.SetAttribute( Attribute_OrigStyle, Element.Style );
                myMarkerIdx = 0;
            }
            else
            {
                myMarkerIdx = int.Parse( markerCountAttr );
            }

            var markupStyle = string.Format( ";color:black;background-color:{0}", ColorTranslator.ToHtml( Color ) );

            Element.SetAttribute( Attribute_Marker + myMarkerIdx, markupStyle );
            Element.SetAttribute( Attribute_MarkerCount, ( myMarkerIdx + 1 ).ToString() );

            Element.Style = Element.GetAttribute( Attribute_OrigStyle ) + markupStyle;
        }

        public void Unmark()
        {
            if( Element == null || Element.Parent == null )
            {
                // if the Parent is null we consider this HtmlElement as to be disconnected from HtmlDocument.
                // (e.g.: System.Windows.Forms.WebBrowser does reuse HtmlDocument instances)
                // -> no need to unmark s.th. - the HtmlElement is dead
                return;
            }

            var markerCount = int.Parse( Element.GetAttribute( Attribute_MarkerCount ) );

            if( myMarkerIdx + 1 == markerCount )
            {
                // no further marker on top of us
                // -> rollback our markup

                // is there still another marker applied?
                string marker = null;
                int previousMarkerIdx = myMarkerIdx - 1;
                for( ; previousMarkerIdx >= 0; previousMarkerIdx-- )
                {
                    marker = Element.GetAttribute( Attribute_Marker + previousMarkerIdx );
                    if( !string.IsNullOrEmpty( marker ) )
                    {
                        break;
                    }
                }

                if( previousMarkerIdx >= 0 )
                {
                    // another marker found
                    // -> apply its style
                    Element.Style = Element.GetAttribute( Attribute_OrigStyle ) + marker;

                    Element.SetAttribute( Attribute_MarkerCount, ( previousMarkerIdx + 1 ).ToString() );
                }
                else
                {
                    // no marker found
                    // -> apply orig style
                    Element.Style = Element.GetAttribute( Attribute_OrigStyle );

                    Element.SetAttribute( Attribute_MarkerCount, ( 0 ).ToString() );
                }
            }
            else
            {
                // another marker on top of us
                // -> nothing to do - just cleanup
            }

            Element.SetAttribute( Attribute_Marker + myMarkerIdx, string.Empty );

            Element = null;
            myMarkerIdx = -1;
        }

        public void Reset()
        {
            Unmark();
        }
    }
}
