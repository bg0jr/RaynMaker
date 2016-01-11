using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
                var result = ExtractTable( myDocument, HtmlPath.Parse( pathSeriesDescriptor.Path ) );
                if( !result.Success )
                {
                    throw new Exception( "Failed to extract table from document: " + result.FailureReason );
                }

                return TableFormatter.ToFormattedTable( pathSeriesDescriptor, result.Value );
            }

            var pathTableDescriptor = myDescriptor as PathTableDescriptor;
            if( pathTableDescriptor != null )
            {
                var result = ExtractTable( myDocument, HtmlPath.Parse( pathTableDescriptor.Path ) );
                if( !result.Success )
                {
                    throw new Exception( "Failed to extract table from document: " + result.FailureReason );
                }

                return TableFormatter.ToFormattedTable( pathTableDescriptor, result.Value );
            }

            var pathCellDescriptor = myDescriptor as PathCellDescriptor;
            if( pathCellDescriptor != null )
            {
                var result = ExtractTable( myDocument, HtmlPath.Parse( pathCellDescriptor.Path ) );
                if( !result.Success )
                {
                    throw new Exception( "Failed to extract table from document: " + result.FailureReason );
                }

                var value = TableFormatter.GetValue( pathCellDescriptor, result.Value );

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
        private FallibleActionResult<DataTable> ExtractTable( IHtmlDocument doc, HtmlPath path )
        {
            Contract.RequiresNotNull( doc, "doc" );
            Contract.RequiresNotNull( path, "path" );

            if( !path.PointsToTable && !path.PointsToTableCell )
            {
                throw new InvalidExpressionException( "Path neither points to table nor to cell" );
            }

            var htmlTable = HtmlTable.GetByPath( doc, path );
            if( htmlTable == null )
            {
                return FallibleActionResult<DataTable>.CreateFailureResult( "Could not get table by path" );
            }

            DataTable table = new DataTable();
            // TODO: should we get the culture from the HTML page somehow?
            table.Locale = CultureInfo.InvariantCulture;

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
                    row[ i ] = htmlRow[ i ].InnerText;
                }
            }

            if( table.Rows.Count == 0 )
            {
                table.Dispose();
                return FallibleActionResult<DataTable>.CreateFailureResult( "Table was empty" );
            }

            return FallibleActionResult<DataTable>.CreateSuccessResult( table );
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
