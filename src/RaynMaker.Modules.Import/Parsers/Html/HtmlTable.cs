using System;
using System.Collections.Generic;
using System.Linq;
using Plainion;
using Plainion.Collections;
using RaynMaker.Modules.Import.Documents;

namespace RaynMaker.Modules.Import.Parsers.Html
{
    public class HtmlTable
    {
        private Lazy<IReadOnlyList<IHtmlElement>> myRows;

        public HtmlTable( IHtmlElement tableElement )
        {
            Contract.RequiresNotNull( tableElement, "tableElement" );
            Contract.Requires( tableElement.TagName.Equals( "TABLE", StringComparison.OrdinalIgnoreCase ), "not a html table element" );

            TableElement = tableElement;

            myRows = new Lazy<IReadOnlyList<IHtmlElement>>( () => GetRows().ToList() );
        }

        public IHtmlElement TableElement { get; private set; }

        /// <summary>
        /// Returns all TR elements of the HTML table including potential header.
        /// </summary>
        public IReadOnlyList<IHtmlElement> Rows
        {
            get { return myRows.Value; }
        }

        private IEnumerable<IHtmlElement> GetRows()
        {
            foreach( var row in TableElement.Children )
            {
                if( row.TagName.Equals( "TR", StringComparison.OrdinalIgnoreCase ) )
                {
                    yield return row;
                }
                if( row.TagName.Equals( "THEAD", StringComparison.OrdinalIgnoreCase ) || row.TagName.Equals( "TBODY", StringComparison.OrdinalIgnoreCase ) )
                {
                    foreach( var innerRow in row.Children )
                    {
                        if( innerRow.TagName.Equals( "TR", StringComparison.OrdinalIgnoreCase ) )
                        {
                            yield return innerRow;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get the content of the row at the specified index.
        /// </summary>
        public IReadOnlyList<IHtmlElement> GetRow( int index )
        {
            Contract.Requires( 0 <= index && index < Rows.Count, "Index out of range" );

            return Rows[ index ].Children;
        }

        /// <summary>
        /// Geth the content of the row of the given cell.
        /// </summary>
        public IReadOnlyList<IHtmlElement> GetRow( IHtmlElement cell )
        {
            Contract.RequiresNotNull( cell, "cell" );

            var row = GetEmbeddingTR( cell );

            Contract.Requires( row != null, "Element does not point to cell inside table row" );

            return row.Children;
        }

        /// <summary>
        /// Returns the row index of the given HtmlElement.
        /// </summary> 
        public int RowIndexOf( IHtmlElement e )
        {
            var tr = GetEmbeddingTR( e );
            if( tr == null )
            {
                return -1;
            }

            return Rows.IndexOf( tr );
        }

        private IHtmlElement GetEmbeddingTR( IHtmlElement element )
        {
            Contract.RequiresNotNull( element, "e" );

            if( element.TagName.Equals( "TR", StringComparison.OrdinalIgnoreCase ) )
            {
                return element;
            }
            else
            {
                var parent = GetParent( element, e => e.TagName.Equals( "TR", StringComparison.OrdinalIgnoreCase ), e => IsTableOrTBody( e ) );
                return ( parent == null ? null : parent );
            }
        }

        private static bool IsTableOrTBody( IHtmlElement element )
        {
            return element.TagName.Equals( "TABLE", StringComparison.OrdinalIgnoreCase ) || element.TagName.Equals( "TBODY", StringComparison.OrdinalIgnoreCase );
        }
        
        public IReadOnlyList<IHtmlElement> GetColumn( int index )
        {
            return Rows
                .Select( row => GetChildAt( row, new[] { "TD", "TH" }, index ) )
                .ToList();
        }

        /// <summary>
        /// Gets the complete column of the given cell.
        /// </summary>
        public IReadOnlyList<IHtmlElement> GetColumn( IHtmlElement cell )
        {
            Contract.RequiresNotNull( cell, "cell" );

            int index = ColumnIndexOf( cell );

            return GetColumn( index );
        }

        /// <summary>
        /// Returns the column index of the given HtmlElement.
        /// </summary>
        public int ColumnIndexOf( IHtmlElement e )
        {
            var td = GetEmbeddingTD( e );
            if( td == null )
            {
                return -1;
            }

            Contract.Invariant( td.Parent != null, "Parent must not be null" );

            int childPos = -1;

            foreach( var child in td.Parent.Children )
            {
                // only count cells (e.g. ignore #text nodes which could be newlines)
                if( !( child.TagName.Equals( "TD", StringComparison.OrdinalIgnoreCase ) || child.TagName.Equals( "TH", StringComparison.OrdinalIgnoreCase ) ) )
                {
                    continue;
                }

                childPos++;

                if( child == td )
                {
                    return childPos;
                }
            }

            return -1;
        }

        private IHtmlElement GetEmbeddingTD( IHtmlElement element )
        {
            Contract.RequiresNotNull( element, "e" );

            if( element.TagName.Equals( "TD", StringComparison.OrdinalIgnoreCase ) || element.TagName.Equals( "TH", StringComparison.OrdinalIgnoreCase ) )
            {
                return element;
            }
            else
            {
                var parent = GetParent( element, e => e.TagName.Equals( "TD", StringComparison.OrdinalIgnoreCase ) || e.TagName.Equals( "TH",
                    StringComparison.OrdinalIgnoreCase ), e => IsTableOrTBody( e ) );
                return ( parent == null ? null : parent );
            }
        }

        private static IHtmlElement GetParent( IHtmlElement start, Predicate<IHtmlElement> cond )
        {
            return GetParent( start, cond, p => false );
        }

        private static IHtmlElement GetParent( IHtmlElement start, Predicate<IHtmlElement> cond, Predicate<IHtmlElement> abortIf )
        {
            Contract.RequiresNotNull( start, "start" );
            Contract.Requires( !abortIf( start ), "start must not be already the target" );

            var parent = start.Parent;
            while( parent != null && !abortIf( parent ) )
            {
                if( cond( parent ) )
                {
                    return parent;
                }

                parent = parent.Parent;
            }

            return null;
        }

        /// <summary>
        /// Returns the TD element at the specified position.
        /// </summary>
        /// <returns>the TD element found, null otherwise</returns>
        public IHtmlElement GetCell( int row, int column )
        {
            var r = Rows.ElementAt( row );
            if( r == null )
            {
                return null;
            }

            return GetChildAt( r, new[] { "TD", "TH" }, column );
        }

        private static IHtmlElement GetChildAt( IHtmlElement parent, string[] tagNames, int pos )
        {
            Contract.RequiresNotNull( parent, "parent" );
            Contract.RequiresNotNullNotEmpty( tagNames, "tagNames" );

            int childPos = 0;
            foreach( var child in parent.Children )
            {
                if( tagNames.Any( t => child.TagName.Equals( t, StringComparison.OrdinalIgnoreCase ) ) )
                {
                    if( childPos == pos )
                    {
                        return child;
                    }
                    childPos++;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the HtmlTable the given path is pointing to.
        /// If the path is pointing into a table, the embedding table is returned.
        /// If the path is not pointing to a table element null is returned.
        /// </summary>
        public static HtmlTable GetByPath( IHtmlDocument doc, HtmlPath path )
        {
            var start = doc.GetElementByPath( path );
            if( start == null )
            {
                return null;
            }

            return GetByElement( start );
        }

        /// <summary>
        /// Searches for the table which embedds the given element.
        /// If the given HtmlElement is a TABLE element, this one is returned.
        /// </summary>
        public static HtmlTable GetByElement( IHtmlElement element )
        {
            Contract.RequiresNotNull( element, "element" );

            if( element.TagName.Equals( "TABLE", StringComparison.OrdinalIgnoreCase ) )
            {
                return new HtmlTable( element );
            }

            var table = GetParent( element, p => p.TagName.Equals( "TABLE", StringComparison.OrdinalIgnoreCase ) );

            return ( table == null ? null : new HtmlTable( table ) );
        }

        /// <summary>
        /// Returns true if given is either TD or TH, false otherwise.
        /// </summary>
        public bool IsCell( IHtmlElement element )
        {
            return element.TagName.Equals( "TD", StringComparison.OrdinalIgnoreCase ) || element.TagName.Equals( "TH", StringComparison.OrdinalIgnoreCase );
        }
    }
}
