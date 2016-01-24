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
        /// Returns the column index of the given HtmlElement or of its surrounding TD element.
        /// </summary>
        public int GetColumnIndex( IHtmlElement e )
        {
            var td = GetEmbeddingTD( e );
            if( td == null )
            {
                return -1;
            }
            return GetChildPos( td );
        }

        private int GetChildPos( IHtmlElement element )
        {
            Contract.RequiresNotNull( element, "element" );

            if( element.Parent == null )
            {
                // assume its valid HTML with <html/> as root element
                return 0;
            }

            int childPos = 0;
            foreach( var child in element.Parent.Children )
            {
                if( child == element )
                {
                    return childPos;
                }
                childPos++;
            }

            return -1;
        }

        private IHtmlElement GetEmbeddingTD( IHtmlElement element )
        {
            Contract.RequiresNotNull( element, "e" );

            if( element.TagName == "TD" || element.TagName == "TH" )
            {
                return element;
            }
            else
            {
                var parent = GetParent( element, e => e.TagName == "TD" || e.TagName == "TH", e => IsTableOrTBody( e ) );
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
        /// Returns the row index of the given HtmlElement or of its
        /// surrounding TR element.
        /// <seealso cref="GetEmbeddingTR"/>
        /// </summary> 
        public int GetRowIndex( IHtmlElement e )
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

            if( element.TagName == "TR" )
            {
                return element;
            }
            else
            {
                var parent = GetParent( element, e => e.TagName == "TR", e => IsTableOrTBody( e ) );
                return ( parent == null ? null : parent );
            }
        }

        /// <summary>
        /// Returns the TD element at the specified position.
        /// </summary>
        /// <returns>the TD element found, null otherwise</returns>
        public IHtmlElement GetCellAt( int row, int column )
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

            // TODO: this could happen if the site has been changed and the 
            // path is no longer valid
            //throw new ArgumentException( "Could not find child for path: " + tagName + "[" + pos + "]" );
        }

        public IReadOnlyList<IHtmlElement> GetRow( int row )
        {
            Contract.Requires( 0 <= row && row < Rows.Count, "Index out of range" );

            return Rows[ row ].Children;
        }

        /// <summary>
        /// Gets the complete row of the given cell.
        /// </summary>
        public IReadOnlyList<IHtmlElement> GetRow( IHtmlElement cell )
        {
            Contract.RequiresNotNull( cell, "cell" );

            var row = GetEmbeddingTR( cell );
            if( row == null )
            {
                throw new ArgumentException( "Element does not point to cell inside table row" );
            }

            return row.Children;
        }

        public IReadOnlyList<IHtmlElement> GetColumn( int col )
        {
            return Rows
                .Select( row => GetRow( row ).ElementAt( col ) )
                .ToList();
        }

        /// <summary>
        /// Gets the complete column of the given cell.
        /// <remarks>Attention: Handling "colspan" is not implemented.
        /// A TR without any TD is skipped.</remarks>
        /// </summary>
        public IReadOnlyList<IHtmlElement> GetColumn( IHtmlElement cell )
        {
            Contract.RequiresNotNull( cell, "cell" );

            // ignore tag - we could have TH and TD in the row
            int colIdx = GetChildPos( cell );

            return Rows
                .Select( row => GetChildAt( row, new[] { "TD", "TH" }, colIdx ) )
                .Where( e => e != null )
                .ToList();
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
        public static HtmlTable GetByElement( IHtmlElement cell )
        {
            Contract.RequiresNotNull( cell, "start" );

            if( cell.TagName == "TABLE" )
            {
                return new HtmlTable( cell );
            }

            var table = GetParent( cell, p => p.TagName == "TABLE" );

            return ( table == null ? null : new HtmlTable( table ) );
        }

        private static bool IsTableOrTBody( IHtmlElement element )
        {
            return element.TagName.Equals( "TABLE", StringComparison.OrdinalIgnoreCase ) || element.TagName.Equals( "TBODY", StringComparison.OrdinalIgnoreCase );
        }

        internal bool IsCell( IHtmlElement td )
        {
            return td.TagName == "TD" || td.TagName == "TH";
        }
    }
}
