using System;
using System.Data;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Parsers.Html;
using RaynMaker.Modules.Import.Spec;
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
                var result = myDocument.ExtractTable( HtmlPath.Parse( pathSeriesDescriptor.Path ) );
                if( !result.Success )
                {
                    throw new Exception( "Failed to extract table from document: " + result.FailureReason );
                }

                return TableFormatter.ToFormattedTable( pathSeriesDescriptor, result.Value );
            }

            var pathTableDescriptor = myDescriptor as PathTableDescriptor;
            if( pathTableDescriptor != null )
            {
                var result = myDocument.ExtractTable( HtmlPath.Parse( pathTableDescriptor.Path ));
                if( !result.Success )
                {
                    throw new Exception( "Failed to extract table from document: " + result.FailureReason );
                }

                return TableFormatter.ToFormattedTable( pathTableDescriptor, result.Value );
            }

            var pathCellDescriptor = myDescriptor as PathCellDescriptor;
            if( pathCellDescriptor != null )
            {
                var result = myDocument.ExtractTable( HtmlPath.Parse( pathCellDescriptor.Path ) );
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
                var str = myDocument.GetTextByPath( HtmlPath.Parse( pathSingleValueDescriptor.Path ) );
                var value = pathSingleValueDescriptor.ValueFormat.Convert( str );

                // XXX: this is really ugly - i have to create a table just to satisfy the interface :(
                return CreateTableForScalar( pathSingleValueDescriptor.ValueFormat.Type, value );
            }

            throw new NotSupportedException( "Format not supported for Html documents: " + myDescriptor.GetType() );
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
