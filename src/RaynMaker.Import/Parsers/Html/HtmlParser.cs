using System;
using System.Data;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Parsers.Html;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2.Extraction;

namespace RaynMaker.Import.Parsers.Html
{
    class HtmlParser : IDocumentParser
    {
        private IHtmlDocument myDocument;
        private IFigureDescriptor myFormat;

        public HtmlParser( IHtmlDocument document, IFigureDescriptor format )
        {
            myDocument = document;
            myFormat = format;
        }

        public DataTable ExtractTable()
        {
            PathSeriesDescriptor pathSeriesFormat = myFormat as PathSeriesDescriptor;
            PathTableDescriptor pathTableFormat = myFormat as PathTableDescriptor;

            if( pathSeriesFormat != null )
            {
                var htmlSettings = new HtmlExtractionSettings();
                htmlSettings.ExtractLinkUrl = pathSeriesFormat.ExtractLinkUrl;

                var result = myDocument.ExtractTable( HtmlPath.Parse( pathSeriesFormat.Path ),
                    TableFormatter.ToExtractionSettings( pathSeriesFormat ), htmlSettings );
                if( !result.Success )
                {
                    throw new Exception( "Failed to extract table from document: " + result.FailureReason );
                }

                return TableFormatter.ToFormattedTable( pathSeriesFormat, result.Value );
            }
            else if( pathTableFormat != null )
            {
                var result = myDocument.ExtractTable( HtmlPath.Parse( pathTableFormat.Path ), true );
                if( !result.Success )
                {
                    throw new Exception( "Failed to extract table from document: " + result.FailureReason );
                }

                return TableFormatter.ToFormattedTable( pathTableFormat, result.Value );
            }
            else if( myFormat is PathSingleValueDescriptor )
            {
                var f = ( PathSingleValueDescriptor )myFormat;
                var str = myDocument.GetTextByPath( HtmlPath.Parse( f.Path ) );
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
