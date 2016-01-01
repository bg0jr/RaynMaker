using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Plainion;
using RaynMaker.Modules.Import.Documents.WinForms;
using RaynMaker.Modules.Import.Parsers.Html;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class HtmlMarker
    {
        private IList<HtmlElement> myMarkedElements;

        public HtmlMarker()
        {
            myMarkedElements = new List<HtmlElement>();
            DefaultColor = Color.Yellow;
        }

        public HtmlDocumentAdapter Document { get; set; }

        public IEnumerable<HtmlElement> MarkedElements { get { return myMarkedElements; } }

        public Color DefaultColor { get; set; }

        public void Mark( HtmlElement e )
        {
            Mark( e, DefaultColor );
        }

        public void Mark( HtmlElement e, Color color )
        {
            Contract.RequiresNotNull( e != null, "e" );

            if( e.TagName == "TABLE" || e.TagName == "TBODY" )
            {
                // not supported
                return;
            }

            if( IsMarked( e ) )
            {
                // unmark first - maybe it was marked with another color before
                Unmark( e );
            }

            if( e.InnerHtml == null )
            {
                // create a pseudo element
                e.AppendChild( e.Document.CreateElement( "SPAN" ) );
                e.InnerText = "&nbsp;";
            }

            string text = e.InnerText == null ? "&nbsp;" : e.InnerText.Trim();

            e.InnerHtml = string.Format( "<SPAN id=\"__rym_markup__\" style=\"color:black;background-color:{0}\">{1}</SPAN>",
                ColorTranslator.ToHtml( color ), text );

            myMarkedElements.Add( e );
            //Debug.WriteLine( GetHashCode() + "Add    : " + e.InnerText );
        }

        public void Unmark( HtmlElement e )
        {
            Contract.RequiresNotNull( e != null, "e" );

            if( !IsMarked( e ) )
            {
                return;
            }

            e.InnerHtml = e.InnerText;

            myMarkedElements.Remove( e );
            //Debug.WriteLine( GetHashCode() + "Remove : " + e.InnerText );
        }

        private bool HasMarkUp( HtmlElement e )
        {
            return ( e.Children.Count > 0 && e.Children[ 0 ].Id == "__rym_markup__" );
        }

        public void UnmarkAll()
        {
            foreach( var e in myMarkedElements.ToList() )
            {
                Unmark( e );
            }

            Contract.Invariant( myMarkedElements.Count == 0, "No element expected to be selected" );
        }

        public bool IsMarked( HtmlElement e )
        {
            Contract.RequiresNotNull( e != null, "e" );

            return myMarkedElements.Contains( e );
        }

        public void MarkTableRow( HtmlElement start )
        {
            MarkTableRow( start, DefaultColor );
        }

        public void MarkTableRow( HtmlElement start, Color color )
        {
            Contract.RequiresNotNull( start != null, "start" );

            var adapter = Document.Create( start );

            if( HtmlTable.GetEmbeddingTR( adapter ) == null )
            {
                // not clicked into table row
                return;
            }

            foreach( var e in HtmlTable.GetRow( adapter ).OfType<HtmlElementAdapter>() )
            {
                Mark( e.Element, color );
            }
        }

        public void MarkTableColumn( HtmlElement start )
        {
            MarkTableColumn( start, DefaultColor );
        }

        public void MarkTableColumn( HtmlElement start, Color color )
        {
            Contract.RequiresNotNull( start != null, "start" );

            var adapter = Document.Create( start );

            if( HtmlTable.GetEmbeddingTD( adapter ) == null )
            {
                // not clicked into table column
                return;
            }

            foreach( var e in HtmlTable.GetColumn( adapter ).OfType<HtmlElementAdapter>() )
            {
                Mark( e.Element, color );
            }
        }
    }
}
