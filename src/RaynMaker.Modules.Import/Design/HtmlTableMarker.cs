using System;
using System.Drawing;
using System.Linq;
using Plainion;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Documents.WinForms;
using RaynMaker.Modules.Import.Parsers.Html;

namespace RaynMaker.Modules.Import.Design
{
    /// <summary>
    /// Highlights one fragment (e.g. one series) within a HTML table using <see cref="HtmlElementMarker"/>.
    /// </summary>
    public class HtmlTableMarker : IHtmlMarker
    {
        public static readonly Color DefaultCellColor = Color.Yellow;
        public static readonly Color DefaultHeaderColor = Color.SteelBlue;

        private HtmlElementAdapter myElement;
        private HtmlElementCollectionMarker myCellMarker;
        private HtmlElementCollectionMarker myHeaderMarker;
        private bool myExpandRow;
        private bool myExpandColumn;
        private int[] mySkipColumns;
        private int[] mySkipRows;
        private int myRowHeaderColumn;
        private int myColumnHeaderRow;

        public HtmlTableMarker()
            : this( DefaultCellColor, DefaultHeaderColor )
        {
        }

        public HtmlTableMarker( Color cellColor, Color headerColor )
        {
            CellColor = cellColor;
            HeaderColor = headerColor;

            myCellMarker = new HtmlElementCollectionMarker( CellColor );
            myHeaderMarker = new HtmlElementCollectionMarker( HeaderColor );

            Reset();
        }

        public Color CellColor { get; private set; }

        public Color HeaderColor { get; private set; }

        public HtmlTable Table { get; private set; }

        /// <summary>
        /// Marks the given HTML table cell element and its siblings according to the expansion settings.
        /// If the given HtmlElement is not within a HTML table this call is ignored.
        /// </summary>
        public void Mark( IHtmlElement element )
        {
            Contract.RequiresNotNull( element, "element" );

            if ( element.FindParent( e => e.TagName.Equals( "TR", StringComparison.OrdinalIgnoreCase ) ) == null )
            {
                return;
            }

            myElement = (HtmlElementAdapter)element;

            Table = HtmlTable.FindByElement( myElement );

            Contract.Requires( Table != null, "Couldnt find <table/> from given HtmlElement" );

            Apply();
        }

        public void Unmark()
        {
            myCellMarker.Unmark();
            myHeaderMarker.Unmark();
        }

        public void Reset()
        {
            Unmark();

            myExpandColumn = false;
            myExpandRow = false;
            mySkipColumns = null;
            mySkipRows = null;
            myRowHeaderColumn = -1;
            myColumnHeaderRow = -1;
        }

        public bool ExpandColumn
        {
            get { return myExpandColumn; }
            set
            {
                myExpandColumn = value;
                Apply();
            }
        }

        public bool ExpandRow
        {
            get { return myExpandRow; }
            set
            {
                myExpandRow = value;
                Apply();
            }
        }

        public int[] SkipRows
        {
            get { return mySkipRows; }
            set
            {
                if ( value != null && value.Length == 0 )
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
                if ( value != null && value.Length == 0 )
                {
                    value = null;
                }
                mySkipColumns = value;
                Apply();
            }
        }

        /// <summary>
        /// Gets or sets the column which defines the header of the row of the highlighted/selected cell.
        /// </summary>
        public int RowHeaderColumn
        {
            get { return myRowHeaderColumn; }
            set
            {
                if ( value < 0 )
                {
                    value = -1;
                }
                myRowHeaderColumn = value;
                Apply();
            }
        }

        /// <summary>
        /// Gets or sets the row which defines the header of the column of the highlighted/selected cell.
        /// </summary>
        public int ColumnHeaderRow
        {
            get { return myColumnHeaderRow; }
            set
            {
                if ( value < 0 )
                {
                    value = -1;
                }
                myColumnHeaderRow = value;
                Apply();
            }
        }

        private void Apply()
        {
            if ( myElement == null )
            {
                return;
            }

            // unmark all first
            Unmark();

            myCellMarker.Mark( myElement );

            if ( Table == null )
            {
                // no table handling
                return;
            }

            if ( ExpandRow )
            {
                MarkTableRow();
                DoSkipColumns();
            }

            if ( ExpandColumn )
            {
                MarkTableColumn();
                DoSkipRows();
            }

            MarkRowHeader();
            MarkColumnHeader();
        }

        private void DoSkipRows()
        {
            int column = Table.GetColumnIndex( myElement );
            if ( column != -1 )
            {
                SkipElements( mySkipRows, row => Table.GetCellAt( row, column ) );
            }
        }

        private void SkipElements( int[] positions, Func<int, IHtmlElement> GetCellAt )
        {
            if ( positions == null )
            {
                return;
            }

            foreach ( var pos in positions )
            {
                myCellMarker.Unmark( GetCellAt( pos ) );
                myHeaderMarker.Unmark( GetCellAt( pos ) );
            }
        }

        private void DoSkipColumns()
        {
            int row = Table.GetRowIndex( myElement );
            if ( row != -1 )
            {
                SkipElements( mySkipColumns, col => Table.GetCellAt( row, col ) );
            }
        }

        // header is everything in the specified RowHeaderColumn along with the expansion of the marked cell
        private void MarkRowHeader()
        {
            if ( myRowHeaderColumn == -1 )
            {
                return;
            }

            var header = myCellMarker.Elements
                  .Select( e => Table.GetCellAt( Table.GetRowIndex( e ), myRowHeaderColumn ) )
                  .Distinct();

            foreach ( var e in header )
            {
                myHeaderMarker.Mark( e );
            }
        }

        // header is everything in the specified ColumnHeaderRow along with the expansion of the marked cell
        private void MarkColumnHeader()
        {
            if ( myColumnHeaderRow == -1 )
            {
                return;
            }

            var header = myCellMarker.Elements
                  .Select( e => Table.GetCellAt( myColumnHeaderRow, Table.GetColumnIndex( e ) ) )
                  // GetCellAt() may return null in case the given coordinates are wrong
                  .Where( e => e != null )
                  .Distinct();

            foreach ( var e in header )
            {
                myHeaderMarker.Mark( e );
            }
        }

        private void MarkTableRow()
        {
            foreach ( var e in Table.GetRow( myElement ) )
            {
                myCellMarker.Mark( e );
            }
        }

        private void MarkTableColumn()
        {
            foreach ( var e in Table.GetColumn( myElement ) )
            {
                myCellMarker.Mark( e );
            }
        }
    }
}