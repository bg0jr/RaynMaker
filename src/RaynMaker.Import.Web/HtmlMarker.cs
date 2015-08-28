using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using Blade;
using Blade.Collections;
using RaynMaker.Import.Html;
using RaynMaker.Import.Html.WinForms;

namespace RaynMaker.Import.Web
{
    /// <summary>
    /// Helper class to work with markups in HTML documents.
    /// <remarks>
    /// It remembers the marked up element to simplify the work with markups.
    /// </remarks>
    /// <seealso cref="HtmlElementExtensions.MarkElement"/>
    /// <seealso cref="HtmlElementExtensions.UnmarkElement"/>
    /// <seealso cref="HtmlElementExtensions.IsMarked"/>
    /// </summary>
    public class HtmlMarker
    {
        private HtmlDocumentAdapter myDocument;

        /// <summary>
        /// Constructor.
        /// <remarks>
        /// Default markup color: yellow.
        /// </remarks>
        /// </summary>
        public HtmlMarker( HtmlDocumentAdapter document)
        {
            myDocument = document;

            MarkedElements = new List<HtmlElement>();
            DefaultColor = Color.Yellow;
        }

        /// <summary>
        /// List of marked elements.
        /// </summary>
        public IList<HtmlElement> MarkedElements { get; private set; }

        /// <summary>
        /// Default color for markups.
        /// </summary>
        public Color DefaultColor { get; set; }

        /// <summary>
        /// Marks the given element.
        /// </summary>
        public void MarkElement( HtmlElement e )
        {
            MarkElement( e, DefaultColor );
        }

        /// <summary>
        /// Marks the given element with the given color.
        /// </summary>
        public void MarkElement( HtmlElement e, Color color )
        {
            e.Require( x => e != null );

            if ( IsMarked( e ) )
            {
                // unmark first - maybe it was marked with another color before
                UnmarkElement( e );
            }

            e.MarkElement( color );

            MarkedElements.Add( e );
        }

        /// <summary>
        /// Removes the markup from the given element if any known.
        /// </summary>
        public void UnmarkElement( HtmlElement e )
        {
            e.Require( x => e != null );

            if ( !IsMarked( e ) )
            {
                return;
            }

            e.UnmarkElement();

            MarkedElements.Remove( e );
        }

        /// <summary>
        /// Removes all the known markups.
        /// </summary>
        public void UnmarkAll()
        {
            MarkedElements.ToList().Foreach( UnmarkElement );
        }

        /// <summary>
        /// Indicates whether the given element is marked.
        /// </summary>
        public bool IsMarked( HtmlElement e )
        {
            e.Require( x => e != null );

            return MarkedElements.Contains( e );
        }

        /// <summary>
        /// Marks the complete row specified by the given table row or cell.
        /// </summary>
        public void MarkTableRow( HtmlElement start )
        {
            MarkTableRow( start, DefaultColor );
        }

        /// <summary>
        /// Marks the complete row specified by the given table row or cell
        /// with the given color.
        /// </summary>
        public void MarkTableRow( HtmlElement start, Color color )
        {
            start.Require( x => start != null );

            HtmlTable.GetRow( myDocument.Create( start ) )
                .OfType<HtmlElementAdapter>()
                .Foreach( e => MarkElement( e.Element, color ) );
        }

        /// <summary>
        /// Marks the complete column specified by the given table row or cell.
        /// </summary>
        public void MarkTableColumn( HtmlElement start )
        {
            MarkTableColumn( start, DefaultColor );
        }

        /// <summary>
        /// Marks the complete column specified by the given table row or cell
        /// with the given color.
        /// </summary>
        public void MarkTableColumn( HtmlElement start, Color color )
        {
            start.Require( x => start != null );

            HtmlTable.GetColumn( myDocument.Create( start ) )
                .OfType<HtmlElementAdapter>()
                .Foreach( e => MarkElement( e.Element, color ) );
        }
    }
}
