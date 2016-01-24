using System;
using System.Data;
using System.Globalization;
using System.Linq;
using Plainion;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.Parsers.Html
{
    class HtmlParser : IDocumentParser
    {
        private IHtmlDocument myDocument;
        private IFigureDescriptor myDescriptor;

        public HtmlParser( IHtmlDocument document, IFigureDescriptor descriptor )
        {
            myDocument = document;
            myDescriptor = descriptor;
        }

        public DataTable ExtractTable()
        {
            var pathSeriesDescriptor = myDescriptor as PathSeriesDescriptor;
            if( pathSeriesDescriptor != null )
            {
                var table = ExtractTable( myDocument, HtmlPath.Parse( pathSeriesDescriptor.Path ) );
                return TableFormatter.ToFormattedTable( pathSeriesDescriptor, table );
            }

            var pathTableDescriptor = myDescriptor as PathTableDescriptor;
            if( pathTableDescriptor != null )
            {
                var table = ExtractTable( myDocument, HtmlPath.Parse( pathTableDescriptor.Path ) );
                return TableFormatter.ToFormattedTable( pathTableDescriptor, table );
            }

            var pathCellDescriptor = myDescriptor as PathCellDescriptor;
            if( pathCellDescriptor != null )
            {
                var table = ExtractTable( myDocument, HtmlPath.Parse( pathCellDescriptor.Path ) );
                var value = TableFormatter.GetValue( pathCellDescriptor, table );

                // XXX: this is really ugly - i have to create a table just to satisfy the interface :(
                return CreateTableForScalar( pathCellDescriptor.ValueFormat.Type, value );
            }

            var pathSingleValueDescriptor = myDescriptor as PathSingleValueDescriptor;
            if( pathSingleValueDescriptor != null )
            {
                var e = myDocument.GetElementByPath( HtmlPath.Parse( pathSingleValueDescriptor.Path ) );
                var str = e == null ? null : e.InnerText;

                var value = pathSingleValueDescriptor.ValueFormat.Convert( str );

                // XXX: this is really ugly - i have to create a table just to satisfy the interface :(
                return CreateTableForScalar( pathSingleValueDescriptor.ValueFormat.Type, value );
            }

            throw new NotSupportedException( "Format not supported for Html documents: " + myDescriptor.GetType() );
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
        private DataTable ExtractTable( IHtmlDocument doc, HtmlPath path )
        {
            Contract.RequiresNotNull( doc, "doc" );
            Contract.RequiresNotNull( path, "path" );

            var htmlTable = HtmlTable.GetByPath( doc, path );
            if( htmlTable == null )
            {
                throw new Exception( "Could not get table by path" );
            }

            var table = new DataTable();
            // TODO: should we get the culture from the HTML page somehow?
            table.Locale = CultureInfo.InvariantCulture;

            foreach( var tr in htmlTable.Rows )
            {
                var rowData = tr.Children
                    .Where( td => htmlTable.IsCell( td ) )
                    .Select( td => td.InnerText )
                    .ToList();

                if( rowData.Count > table.Columns.Count )
                {
                    ( rowData.Count - table.Columns.Count ).Times( x => table.Columns.Add( string.Empty, typeof( object ) ) );
                }

                var row = table.NewRow();
                table.Rows.Add( row );
                table.AcceptChanges();

                for( int i = 0; i < rowData.Count; ++i )
                {
                    row[ i ] = rowData[ i ];
                }
            }

            if( table.Rows.Count == 0 )
            {
                table.Dispose();
                throw new Exception( "Table was empty" );
            }

            return table;
        }

        private DataTable CreateTableForScalar( Type dataType, object value )
        {
            var table = new DataTable();

            table.Columns.Add( new DataColumn( "value", dataType ) );
            table.Rows.Add( table.NewRow() );
            table.AcceptChanges();

            table.Rows[ 0 ][ 0 ] = value;

            return table;
        }
    }
}
