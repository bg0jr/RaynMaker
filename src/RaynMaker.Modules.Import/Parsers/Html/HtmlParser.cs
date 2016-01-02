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
            PathSeriesDescriptor pathSeriesDescriptor = myDescriptor as PathSeriesDescriptor;
            PathTableDescriptor pathTableDescriptor = myDescriptor as PathTableDescriptor;

            if( pathSeriesDescriptor != null )
            {
                var result = myDocument.ExtractTable( HtmlPath.Parse( pathSeriesDescriptor.Path ) );
                if( !result.Success )
                {
                    throw new Exception( "Failed to extract table from document: " + result.FailureReason );
                }

                return TableFormatter.ToFormattedTable( pathSeriesDescriptor, result.Value );
            }
            else if( pathTableDescriptor != null )
            {
                var result = myDocument.ExtractTable( HtmlPath.Parse( pathTableDescriptor.Path ), true );
                if( !result.Success )
                {
                    throw new Exception( "Failed to extract table from document: " + result.FailureReason );
                }

                return TableFormatter.ToFormattedTable( pathTableDescriptor, result.Value );
            }
            else if( myDescriptor is PathSingleValueDescriptor )
            {
                var f = ( PathSingleValueDescriptor )myDescriptor;
                var str = myDocument.GetTextByPath( HtmlPath.Parse( f.Path ) );
                var value = f.ValueFormat.Convert( str );

                // XXX: this is really ugly - i have to create a table just to satisfy the interface :(
                return CreateTableForScalar( f.ValueFormat.Type, value );
            }
            else
            {
                throw new NotSupportedException( "Format not supported for Html documents: " + myDescriptor.GetType() );
            }
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
