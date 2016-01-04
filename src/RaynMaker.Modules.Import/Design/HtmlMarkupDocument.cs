using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Plainion;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Documents.WinForms;
using RaynMaker.Modules.Import.Parsers.Html;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.Design
{
    /// <summary>
    /// - "Marker" - marks one element and remembers the element so that we can unmark again
    /// - "Markers" - collection of all markers
    /// - "Pen" - which does the markup and has logic to mark table rows and columns
    /// </summary>
    public class HtmlMarkupDocument : IDisposable
    {
        private HtmlMarker myMarker = null;
        private HtmlDocumentAdapter myDocument = null;
        // holds the element which has been marked by the user "click"
        // (before extensions has been applied)
        private HtmlElementAdapter mySelectedElement = null;
        private HtmlTable myTable = null;
        private string myAnchor = null;
        private SeriesOrientation myDimension;
        private int[] mySkipColumns = null;
        private int[] mySkipRows = null;
        private int myRowHeader = -1;
        private int myColumnHeader = -1;
        private string mySeriesName = null;

        public HtmlMarkupDocument()
        {
            myMarker = new HtmlMarker();
            Reset();
        }

        public Action SelectionChanged = null;

        public HtmlElementAdapter SelectedElement
        {
            get { return mySelectedElement; }
            set
            {
                myMarker.UnmarkAll();

                mySelectedElement = value;

                if( mySelectedElement == null )
                {
                    myTable = null;
                }
                else
                {
                    myTable = mySelectedElement.FindEmbeddingTable();

                    Apply();

                    mySelectedElement.Element.ScrollIntoView( false );
                }

                if( SelectionChanged != null )
                {
                    SelectionChanged();
                }
            }
        }

        public HtmlDocument Document
        {
            get { return myDocument != null ? myDocument.Document : null; }
            set
            {
                if( myDocument != null )
                {
                    //Debug.WriteLine( GetHashCode() + ": Remove OnClick" );
                    myDocument.Document.Click -= HtmlDocument_Click;
                }

                if( value == null )
                {
                    return;
                }

                myDocument = new HtmlDocumentAdapter( value );
                //Debug.WriteLine( GetHashCode() + ": Add OnClick" );
                myDocument.Document.Click += HtmlDocument_Click;

                // Internally adjusts SelectedElement
                Anchor = Anchor;
            }
        }

        private void HtmlDocument_Click( object sender, HtmlElementEventArgs e )
        {
            //Debug.WriteLine( GetHashCode() + ": OnClick" );

            var element = myDocument.Document.GetElementFromPoint( e.ClientMousePosition );

            if( myMarker.IsMarked( element.Parent ) )
            {
                element = element.Parent;
            }

            var adapter = myDocument.Create( element );
            SelectedElement = adapter;
        }

        public string Anchor
        {
            get { return myAnchor; }
            set
            {
                myAnchor = value;

                UpdateSelectedElement();
            }
        }

        private void UpdateSelectedElement()
        {
            if( myDocument != null && myAnchor != null )
            {
                var path = HtmlPath.TryParse( myAnchor );
                if( path == null )
                {
                    // TODO: signal error to UI
                    return;
                }
                SelectedElement = ( HtmlElementAdapter )myDocument.GetElementByPath( path );
            }
            else
            {
                SelectedElement = null;
            }
        }

        public void Dispose()
        {
            if( myDocument != null )
            {
                myDocument.Document.Click -= HtmlDocument_Click;
            }
        }

        public void Reset()
        {
            myMarker.UnmarkAll();

            mySelectedElement = null;
            myTable = null;

            myDimension = SeriesOrientation.Row;

            mySkipColumns = null;
            mySkipRows = null;
            myRowHeader = -1;
            myColumnHeader = -1;
            mySeriesName = null;
        }

        public SeriesOrientation Dimension
        {
            get { return myDimension; }
            set
            {
                myDimension = value;
                Apply();
            }
        }

        public int[] SkipRows
        {
            get { return mySkipRows; }
            set
            {
                if( value != null && value.Length == 0 )
                {
                    value = null;
                }
                mySkipRows = value;
                Apply();
            }
        }

        public int[] SkipColumns
        {
            get { return mySkipColumns; }
            set
            {
                if( value != null && value.Length == 0 )
                {
                    value = null;
                }
                mySkipColumns = value;
                Apply();
            }
        }

        public int RowHeaderColumn
        {
            get { return myRowHeader; }
            set
            {
                if( value < 0 )
                {
                    value = -1;
                }
                myRowHeader = value;
                Apply();
            }
        }

        public int ColumnHeaderRow
        {
            get { return myColumnHeader; }
            set
            {
                if( value < 0 )
                {
                    value = -1;
                }
                myColumnHeader = value;
                Apply();
            }
        }

        public string SeriesName
        {
            get { return mySeriesName; }
            set
            {
                mySeriesName = value;
                ValidateSeriesName();
            }
        }

        public void Apply()
        {
            if( mySelectedElement == null )
            {
                UpdateSelectedElement();
                return;
            }

            if( mySelectedElement == null || mySelectedElement.TagName.Equals( "INPUT", StringComparison.OrdinalIgnoreCase ) )
            {
                return;
            }

            // unmark all first
            myMarker.UnmarkAll();

            if( myDimension == SeriesOrientation.Row )
            {
                MarkTableRow( mySelectedElement.Element );
                DoSkipColumns();
            }
            else if( myDimension == SeriesOrientation.Column )
            {
                MarkTableColumn( mySelectedElement.Element );
                DoSkipRows();
            }
            else
            {
                myMarker.Mark( mySelectedElement.Element );
            }

            MarkRowHeader();
            MarkColumnHeader();

            ValidateSeriesName();
        }

        private void ValidateSeriesName()
        {
            if( mySelectedElement == null || mySeriesName == null )
            {
                return;
            }

            IHtmlElement header = null;
            if( myDimension == SeriesOrientation.Column )
            {
                if( myColumnHeader != -1 )
                {
                    header = FindColumnHeader( myColumnHeader )( mySelectedElement );
                }
            }
            else
            {
                // row or cell
                if( myRowHeader != -1 )
                {
                    header = FindRowHeader( myRowHeader )( mySelectedElement );
                }
            }
        }

        private void DoSkipRows()
        {
            int column = HtmlTable.GetColumnIndex( mySelectedElement );
            if( column == -1 )
            {
                return;
            }

            Func<int, IHtmlElement> GetCellAt = row => myTable.GetCellAt( row, column );

            SkipElements( mySkipRows, GetCellAt );
        }

        private void DoSkipColumns()
        {
            int row = HtmlTable.GetRowIndex( mySelectedElement );
            if( row == -1 )
            {
                return;
            }

            Func<int, IHtmlElement> GetCellAt = col => myTable.GetCellAt( row, col );

            SkipElements( mySkipColumns, GetCellAt );
        }

        public Func<IHtmlElement, IHtmlElement> FindRowHeader( int pos )
        {
            return e => myTable.GetCellAt( HtmlTable.GetRowIndex( e ), pos );
        }

        public Func<IHtmlElement, IHtmlElement> FindColumnHeader( int pos )
        {
            return e => myTable.GetCellAt( pos, HtmlTable.GetColumnIndex( e ) );
        }

        private void MarkRowHeader()
        {
            MarkHeader( myRowHeader, FindRowHeader );
        }

        private void MarkColumnHeader()
        {
            MarkHeader( myColumnHeader, FindColumnHeader );
        }

        private void MarkHeader( int pos, Func<int, Func<IHtmlElement, IHtmlElement>> FindHeaderCreator )
        {
            if( pos == -1 )
            {
                return;
            }

            var FindHeader = FindHeaderCreator( pos );

            List<IHtmlElement> header = null;
            //if( myDimension == SeriesOrientation.None )
            //{
            //    // mark single column/row 
            //    header = new List<IHtmlElement>();
            //    header.Add( FindHeader( mySelectedElement ) );
            //}
            //else
            {
                // mark all columns/rows
                header = myMarker.MarkedElements
                    .Select( m => myDocument.Create( m.Value ) )
                    .Select( m => FindHeader( m ) )
                    .Distinct()
                    .ToList();
            }

            foreach( var e in header.Cast<HtmlElementAdapter>() )
            {
                myMarker.Mark( e.Element, Color.SteelBlue );
            }
        }

        private void SkipElements( int[] positions, Func<int, IHtmlElement> GetCellAt )
        {
            if( positions == null )
            {
                return;
            }

            foreach( var pos in positions )
            {
                myMarker.Unmark( ( ( HtmlElementAdapter )GetCellAt( pos ) ).Element );
            }
        }

        public void UnmarkAll()
        {
            myMarker.UnmarkAll();
        }

        private void MarkTableRow( HtmlElement start )
        {
            MarkTableRow( start, HtmlMarker.DefaultColor );
        }

        private void MarkTableRow( HtmlElement start, Color color )
        {
            Contract.RequiresNotNull( start != null, "start" );

            var adapter = myDocument.Create( start );

            if( HtmlTable.GetEmbeddingTR( adapter ) == null )
            {
                // not clicked into table row
                return;
            }

            foreach( var e in HtmlTable.GetRow( adapter ).OfType<HtmlElementAdapter>() )
            {
                myMarker.Mark( e.Element, color );
            }
        }

        private void MarkTableColumn( HtmlElement start )
        {
            MarkTableColumn( start, HtmlMarker.DefaultColor );
        }

        private void MarkTableColumn( HtmlElement start, Color color )
        {
            Contract.RequiresNotNull( start != null, "start" );

            var adapter = myDocument.Create( start );

            if( HtmlTable.GetEmbeddingTD( adapter ) == null )
            {
                // not clicked into table column
                return;
            }

            foreach( var e in HtmlTable.GetColumn( adapter ).OfType<HtmlElementAdapter>() )
            {
                myMarker.Mark( e.Element, color );
            }
        }
    }
}