using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using Plainion;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Parsers.Html
{
    /// <summary>
    /// Extensions for the Windows.Forms.HtmlDocument class.
    /// </summary>
    public static class HtmlDocumentExtensions
    {
        /// <summary>
        /// Returns the element specified by the given <see cref="HtmlPath"/>.
        /// </summary>
        public static IHtmlElement GetElementByPath( this IHtmlDocument doc, HtmlPath path )
        {
            Contract.RequiresNotNull( doc, "doc" );
            Contract.RequiresNotNull( path, "path" );

            var root = doc.Body.GetRoot();
            if( root == null )
            {
                return null;
            }

            foreach( var element in path.Elements )
            {
                root = root.GetChildAt( element.TagName, element.Position );

                if( root == null )
                {
                    return null;
                }
            }

            return root;
        }

        /// <summary>
        /// Gets the text of the element specified by the given <see cref="HtmlPath"/>.
        /// </summary>
        public static string GetTextByPath( this IHtmlDocument doc, HtmlPath path )
        {
            var e = doc.GetElementByPath( path );
            if( e == null )
            {
                return null;
            }

            return e.InnerText;
        }

        /// <summary>
        /// Gets the HtmlTable the given path is pointing to.
        /// If the path is pointing into a table, the embedding table is returned.
        /// If the path is not pointing to a table element null is returned.
        /// </summary>
        public static HtmlTable GetTableByPath( this IHtmlDocument doc, HtmlPath path )
        {
            var start = doc.GetElementByPath( path );
            if( start == null )
            {
                return null;
            }

            return start.FindEmbeddingTable();
        }

        public static HtmlForm GetFormByName( this IHtmlDocument document, string formName )
        {
            return new HtmlForm( document.Body.GetFormByName( formName ) );
        }

        private static IHtmlElement GetFormByName( this IHtmlElement element, string formName )
        {
            foreach( var child in element.Children )
            {
                if( child.TagName.Equals( "form", StringComparison.OrdinalIgnoreCase )
                    && child.GetAttribute( "name" ).Equals( formName, StringComparison.OrdinalIgnoreCase ) )
                {
                    return child;
                }
                else
                {
                    var form = child.GetFormByName( formName );
                    if( form != null )
                    {
                        return form;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Extracts the complete html table the given path is pointing to. If the path points
        /// to a cell of a table the complete table is extracted still.
        /// <remarks>
        /// Returns null if table not found by path. Currently we cannot handle thead 
        /// and tfoot. The number of the column is defined by the html table row with the most
        /// html columns
        /// </remarks>
        /// </summary>
        /// <param name="doc">the HTML document</param>
        /// <param name="path">the path to the table</param>
        /// <param name="textOnly">set this to true to get only the text of the cell, otherwise the
        /// cell itself as HtmlElement is returned</param>
        internal static FallibleActionResult<DataTable> ExtractTable( this IHtmlDocument doc, HtmlPath path, bool textOnly )
        {
            Contract.RequiresNotNull( doc, "doc" );
            Contract.RequiresNotNull( path, "path" );

            HtmlTable htmlTable = doc.GetTableByPath( path );
            if( htmlTable == null )
            {
                return FallibleActionResult<DataTable>.CreateFailureResult( "Could not get table by path" );
            }

            DataTable table = new DataTable();
            // TODO: should we get the culture from the HTML page somehow?
            table.Locale = CultureInfo.InvariantCulture;

            Func<IHtmlElement, object> GetContent = element => ( textOnly ? ( object )element.InnerText : element );

            foreach( var tr in htmlTable.Rows )
            {
                var htmlRow = new List<IHtmlElement>();
                foreach( var td in tr.Children )
                {
                    if( td.TagName == "TD" || td.TagName == "TH" )
                    {
                        htmlRow.Add( td );
                    }
                }

                // add columns if necessary
                if( htmlRow.Count > table.Columns.Count )
                {
                    ( htmlRow.Count - table.Columns.Count ).Times( x => table.Columns.Add( string.Empty, typeof( object ) ) );
                }

                // add new row to table
                DataRow row = table.NewRow();
                table.Rows.Add( row );
                table.AcceptChanges();

                // add data
                for( int i = 0; i < htmlRow.Count; ++i )
                {
                    row[ i ] = GetContent( htmlRow[ i ] );
                }
            }

            if( table.Rows.Count == 0 )
            {
                table.Dispose();
                return FallibleActionResult<DataTable>.CreateFailureResult( "Table was empty" );
            }

            return FallibleActionResult<DataTable>.CreateSuccessResult( table );
        }

        /// <summary>
        /// Extracts a cell or a series of the html table the given path is pointing to. 
        /// If the path points to the table element itself instead of a cell the whole table will be 
        /// extracted.
        /// The series to be extracted is always arranged in a column (independed of the original layout
        /// in the html table). The first column contains the values, the second the series header (if
        /// any defined). The series name is stored in the ColumnName of the first column.
        /// <remarks>
        /// Returns null if table not found by path. Currently we cannot handle thead 
        /// and tfoot. 
        /// </remarks>
        /// </summary>
        /// <param name="path">points to a cell or the body of a table (pointers to TR elements are invalid)</param>
        /// <param name="doc">the HTML document</param>
        /// <param name="htmlSettings">the HTML settings used to configure the extraction process</param>
        /// <param name="tableSettings">the table specific configuration</param>
        internal static FallibleActionResult<DataTable> ExtractTable( this IHtmlDocument doc, HtmlPath path, TableExtractionSettings tableSettings )
        {
            if( !path.PointsToTable && !path.PointsToTableCell )
            {
                throw new InvalidExpressionException( "Path neither points to table nor to cell" );
            }

            var result = ExtractTable( doc, path, true );
            if( !result.Success )
            {
                // pass throu failure result
                return result;
            }

            // path points to whole table => return whole table
            if( path.PointsToTable )
            {
                return result;
            }

            // get the x,y position of the cell the path is pointing to
            Point cellCoords = path.GetTableCellPosition();
            if( cellCoords.X < 0 || cellCoords.Y < 0 )
            {
                throw new InvalidExpressionException( "Path expression corrupt: cell position in table could not be calculated" );
            }

            // get the value of the raw cell. extract the link url if configured.
            Func<object, object> GetValue = e => e;

            var t = result.Value.ExtractSeries( cellCoords, GetValue, tableSettings );
            if( t == null )
            {
                return FallibleActionResult<DataTable>.CreateFailureResult( "Could not extract series specified" );
            }

            return FallibleActionResult<DataTable>.CreateSuccessResult( t );
        }
    }
}
