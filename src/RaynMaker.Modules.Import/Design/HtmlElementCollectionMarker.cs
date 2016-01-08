using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Plainion;
using RaynMaker.Modules.Import.Documents;

namespace RaynMaker.Modules.Import.Design
{
    /// <summary>
    /// Hightlights a collection of HTML elements using <see cref="HtmlElementMarker"/>.
    /// </summary>
    public class HtmlElementCollectionMarker : IHtmlMarker, IEnumerable<HtmlElementMarker>
    {
        private IList<HtmlElementMarker> myMarkers;

        public HtmlElementCollectionMarker( Color color )
        {
            Color = color;

            myMarkers = new List<HtmlElementMarker>();
        }

        public Color Color { get; private set; }

        public IEnumerable<IHtmlElement> Elements { get { return myMarkers.Select( m => m.Element ); } }

        public void Mark( IHtmlElement element )
        {
            Contract.RequiresNotNull( element, "element" );

            if( myMarkers.Any( m => m.Element == element ) )
            {
                return;
            }

            var marker = new HtmlElementMarker( Color );
            marker.Mark( element );

            myMarkers.Add( marker );
        }

        public void Unmark( IHtmlElement element )
        {
            Contract.RequiresNotNull( element, "element" );

            var marker = myMarkers.FirstOrDefault( m => m.Element == element );
            if( marker == null )
            {
                return;
            }

            marker.Unmark();

            myMarkers.Remove( marker );
        }

        public void Unmark()
        {
            foreach( var marker in myMarkers )
            {
                marker.Unmark();
            }
            myMarkers.Clear();
        }

        public void Reset()
        {
            Unmark();
        }
        
        public IEnumerator<HtmlElementMarker> GetEnumerator()
        {
            return myMarkers.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ( ( IEnumerable )myMarkers ).GetEnumerator();
        }
    }
}
