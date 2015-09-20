using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RaynMaker.Import.Parsers.Html;
using RaynMaker.Import.Parsers.Html.WinForms;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Web.ViewModels
{
    public class MarkupDocument : IDisposable
    {
        private HtmlMarker myMarker = null;
        private HtmlDocumentAdapter myDocument = null;
        // holds the element which has been marked by the user "click"
        // (before extensions has been applied)
        private HtmlElementAdapter mySelectedElement = null;
        private HtmlTable myTable = null;
        private string myAnchor = null;
        private CellDimension myDimension = CellDimension.None;
        private int[] mySkipColumns = null;
        private int[] mySkipRows = null;
        private int myRowHeader = -1;
        private int myColumnHeader = -1;
        private string mySeriesName = null;

        public MarkupDocument()
        {
            myMarker = new HtmlMarker();
            Reset();
        }

        public Action<bool> ValidationChanged = null;
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

                myMarker.Document = myDocument;

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

                if( myDocument != null && myAnchor != null )
                {
                    var path = HtmlPath.TryParse( value );
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

            myDimension = CellDimension.None;

            mySkipColumns = null;
            mySkipRows = null;
            myRowHeader = -1;
            myColumnHeader = -1;
            mySeriesName = null;
        }

        public CellDimension Dimension
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
            if( mySelectedElement == null || mySelectedElement.TagName.Equals( "INPUT", StringComparison.OrdinalIgnoreCase ) )
            {
                return;
            }

            // unmark all first
            myMarker.UnmarkAll();

            if( myDimension == CellDimension.Row )
            {
                myMarker.MarkTableRow( mySelectedElement.Element );
                DoSkipColumns();
            }
            else if( myDimension == CellDimension.Column )
            {
                myMarker.MarkTableColumn( mySelectedElement.Element );
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
            if( myDimension == CellDimension.Column )
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

            if( header != null && !header.InnerText.Contains( mySeriesName ) )
            {
                FireValidationChanged( false );
            }
            else
            {
                FireValidationChanged( true );
            }
        }

        private void FireValidationChanged( bool isValid )
        {
            if( ValidationChanged != null )
            {
                ValidationChanged( isValid );
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

        private Func<IHtmlElement, IHtmlElement> FindRowHeader( int pos )
        {
            return e => myTable.GetCellAt( HtmlTable.GetRowIndex( e ), pos );
        }

        private Func<IHtmlElement, IHtmlElement> FindColumnHeader( int pos )
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
            if( myDimension == CellDimension.None )
            {
                // mark single column/row 
                header = new List<IHtmlElement>();
                header.Add( FindHeader( mySelectedElement ) );
            }
            else
            {
                // mark all columns/rows
                header = myMarker.MarkedElements
                    .Select( m => myDocument.Create( m ) )
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
    }
}