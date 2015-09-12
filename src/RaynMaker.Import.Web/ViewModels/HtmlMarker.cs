using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Blade;
using Blade.Collections;
using Plainion;
using RaynMaker.Import.Html;
using RaynMaker.Import.Html.WinForms;

namespace RaynMaker.Import.Web.ViewModels
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
        /// <summary>
        /// Constructor.
        /// <remarks>
        /// Default markup color: yellow.
        /// </remarks>
        /// </summary>
        public HtmlMarker( )
        {
            MarkedElements = new List<HtmlElement>();
            DefaultColor = Color.Yellow;
        }

        public HtmlDocumentAdapter Document {get;set;}
        
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
            Contract.RequiresNotNull( e != null, "e" );

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
            Contract.RequiresNotNull( e != null, "e" );

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
            Contract.RequiresNotNull( e != null, "e" );

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
            Contract.RequiresNotNull( start != null, "start" );

            HtmlTable.GetRow( Document.Create( start ) )
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
            Contract.RequiresNotNull( start != null, "start" );

            HtmlTable.GetColumn( Document.Create( start ) )
                .OfType<HtmlElementAdapter>()
                .Foreach( e => MarkElement( e.Element, color ) );
        }
    }
}
