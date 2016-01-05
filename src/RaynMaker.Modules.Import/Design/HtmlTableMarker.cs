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
        public static readonly Color DefaultCellColor = Color.Yellow;
        public static readonly Color DefaultHeaderColor = Color.SteelBlue;

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

            myElement = ( HtmlElementAdapter )element;

            myTable = HtmlTable.FindByCell( myElement );

            // As the using code has to decide to take this marker or another impl we should handle 
            // "wrong elements" a bit relaxed here. Otherwise we force the using code to put checks and if-then-else stuff.
            //Contract.Requires( myTable != null, "given HtmlElement does not point to or into <table/>" );

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

        private void Apply()
        {
            if( myElement == null )
            {
                return;
            }

            // unmark all first
            Unmark();

            myCellMarker.Mark( myElement );

            if( myTable == null )
            {
                // no table handling
                return;
            }

            if( ExpandRow )
            {
                MarkTableRow();
                DoSkipColumns();
            }

            if( ExpandColumn )
            {
                MarkTableColumn();
                DoSkipRows();
            }

            MarkRowHeader();
            MarkColumnHeader();
        }

        private void DoSkipRows()
        {
            int column = myTable.GetColumnIndex( myElement );
            if( column != -1 )
            {
                SkipElements( mySkipRows, row => myTable.GetCellAt( row, column ) );
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

        private void DoSkipColumns()
        {
            int row = myTable.GetRowIndex( myElement );
            if( row != -1 )
            {
                SkipElements( mySkipColumns, col => myTable.GetCellAt( row, col ) );
            }
        }

        public Func<IHtmlElement, IHtmlElement> FindRowHeader( int pos )
        {
            return e => myTable.GetCellAt( myTable.GetRowIndex( e ), pos );
        }

        public Func<IHtmlElement, IHtmlElement> FindColumnHeader( int pos )
        {
            return e => myTable.GetCellAt( pos, myTable.GetColumnIndex( e ) );
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

            // mark all columns/rows
            var header = myCellMarker.Elements
                  .Select( e => FindHeader( e ) )
                  .Distinct()
                  .ToList();

            foreach( var e in header )
            {
                myHeaderMarker.Mark( e );
            }
        }

        private void MarkTableRow()
        {
            foreach( var e in myTable.GetRow( myElement ) )
            {
                myCellMarker.Mark( e );
            }
        }

        private void MarkTableColumn()
        {
            foreach( var e in myTable.GetColumn( myElement ) )
            {
                myCellMarker.Mark( e );
            }
        }
    }
}