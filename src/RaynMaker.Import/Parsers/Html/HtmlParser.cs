using System;
using System.Data;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Parsers.Html;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Parsers.Html
{
    class HtmlParser : IDocumentParser
    {
        private HtmlDocumentHandle myDocument;
        private IFormat myFormat;

        public HtmlParser( HtmlDocumentHandle htmlDocument, IFormat format )
        {
            myDocument = htmlDocument;
            myFormat = format;
        }

        public DataTable ExtractTable()
        {
            PathSeriesFormat pathSeriesFormat = myFormat as PathSeriesFormat;
            PathTableFormat pathTableFormat = myFormat as PathTableFormat;

            if( pathSeriesFormat != null )
            {
                var htmlSettings = new HtmlExtractionSettings();
                htmlSettings.ExtractLinkUrl = pathSeriesFormat.ExtractLinkUrl;

                var result = myDocument.Content.ExtractTable( HtmlPath.Parse( pathSeriesFormat.Path ), pathSeriesFormat.ToExtractionSettings(), htmlSettings );
                if( !result.Success )
                {
                    throw new Exception( "Failed to extract table from document: " + result.FailureReason );
                }

                return pathSeriesFormat.ToFormattedTable( result.Value );
            }
            else if( pathTableFormat != null )
            {
                var result = myDocument.Content.ExtractTable( HtmlPath.Parse( pathTableFormat.Path ), true );
                if( !result.Success )
                {
                    throw new Exception( "Failed to extract table from document: " + result.FailureReason );
                }

                return pathTableFormat.ToFormattedTable( result.Value );
            }
            else if( myFormat is PathSingleValueFormat )
            {
                var f = ( PathSingleValueFormat )myFormat;
                var str = myDocument.Content.GetTextByPath( HtmlPath.Parse( f.Path ) );
                var value = f.ValueFormat.Convert( str );

                // XXX: this is really ugly - i have to create a table just to satisfy the interface :(
                return CreateTableForScalar( f.ValueFormat.Type, value );
            }
            else
            {
                throw new NotSupportedException( "Format not supported for Html documents: " + myFormat.GetType() );
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
