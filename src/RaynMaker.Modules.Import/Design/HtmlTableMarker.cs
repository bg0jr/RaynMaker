using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Plainion;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Documents.WinForms;
using RaynMaker.Modules.Import.Parsers.Html;

namespace RaynMaker.Modules.Import.Design
{
    public class HtmlTableMarker : IHtmlMarker
    {
        public static Color DefaultCellColor = Color.Yellow;
        public static Color DefaultHeaderColor = Color.SteelBlue;

        private HtmlElementAdapter myElement;
        private HtmlTable myTable;
        private HtmlElementCollectionMarker myCellMarker;
        private HtmlElementCollectionMarker myHeaderMarker;
        private bool myExpandRow;
        private bool myExpandColumn;
        private int[] mySkipColumns;
        private int[] mySkipRows;
        private int myRowHeader;
        private int myColumnHeader;

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

        public void Mark( IHtmlElement element )
        {
            Contract.RequiresNotNull( element, "element" );

            if( myElement != null )
            {
                myCellMarker.Unmark();
            }

            myElement = ( HtmlElementAdapter )element;

            myTable = myElement.FindEmbeddingTable();

            Contract.Requires( myTable != null, "given HtmlElement does not point to or into <table/>" );

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
            myRowHeader = -1;
            myColumnHeader = -1;
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

        public void Apply()
        {
            if( myElement == null )
            {
                return;
            }

            // unmark all first
            Unmark();

            if( ExpandRow )
            {
                MarkTableRow( myElement.Element );
                DoSkipColumns();
            }

            if( ExpandColumn )
            {
                MarkTableColumn( myElement.Element );
                DoSkipRows();
            }

            myCellMarker.Mark( myElement );

            MarkRowHeader();
            MarkColumnHeader();
        }

        private void DoSkipRows()
        {
            int column = HtmlTable.GetColumnIndex( myElement );
            if( column == -1 )
            {
                return;
            }

            Func<int, IHtmlElement> GetCellAt = row => myTable.GetCellAt( row, column );

            SkipElements( mySkipRows, GetCellAt );
        }

        private void DoSkipColumns()
        {
            int row = HtmlTable.GetRowIndex( myElement );
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
                header = myCellMarker.Elements
                    .Select( e => FindHeader( e ) )
                    .Distinct()
                    .ToList();
            }

            foreach( var e in header.Cast<HtmlElementAdapter>() )
            {
                myHeaderMarker.Mark( e );
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
                myCellMarker.Unmark( GetCellAt( pos ) );
                myHeaderMarker.Unmark( GetCellAt( pos ) );
            }
        }


        private void MarkTableRow( HtmlElement start )
        {
            var adapter = myElement.DocumentAdapter.Create( start );

            if( HtmlTable.GetEmbeddingTR( adapter ) == null )
            {
                // not clicked into table row
                return;
            }

            foreach( var e in HtmlTable.GetRow( adapter ) )
            {
                myCellMarker.Mark( e );
            }
        }

        private void MarkTableColumn( HtmlElement start )
        {
            var adapter = myElement.DocumentAdapter.Create( start );

            if( HtmlTable.GetEmbeddingTD( adapter ) == null )
            {
                // not clicked into table column
                return;
            }

            foreach( var e in HtmlTable.GetColumn( adapter ) )
            {
                myCellMarker.Mark( e );
            }
        }
    }
}