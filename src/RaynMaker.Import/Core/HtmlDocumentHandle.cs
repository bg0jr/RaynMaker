using System;
using System.Data;
using System.Windows.Forms;
using RaynMaker.Import.Html;
using RaynMaker.Import.Html.WinForms;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Core
{
    public class HtmlDocumentHandle : IDocument
    {
        public HtmlDocumentHandle( IHtmlDocument doc )
        {
            Content = doc;
            Location = doc.Url.ToString();
        }

        public string Location { get; private set; }
        public IHtmlDocument Content { get; private set; }

        public DataTable ExtractTable( IFormat format )
        {
            PathSeriesFormat pathSeriesFormat = format as PathSeriesFormat;
            PathTableFormat pathTableFormat = format as PathTableFormat;

            if ( pathSeriesFormat != null )
            {
                var htmlSettings = new HtmlExtractionSettings();
                htmlSettings.ExtractLinkUrl = pathSeriesFormat.ExtractLinkUrl;

                var result = Content.ExtractTable( HtmlPath.Parse( pathSeriesFormat.Path ), pathSeriesFormat.ToExtractionSettings(), htmlSettings );
                if ( !result.Success )
                {
                    throw new Exception( "Failed to extract table from document: " + result.FailureReason );
                }

                return pathSeriesFormat.ToFormattedTable( result.Value );
            }
            else if ( pathTableFormat != null )
            {
                var result = Content.ExtractTable( HtmlPath.Parse( pathTableFormat.Path ), true );
                if ( !result.Success )
                {
                    throw new Exception( "Failed to extract table from document: " + result.FailureReason );
                }

                return pathTableFormat.ToFormattedTable( result.Value );
            }
            else if ( format is PathSingleValueFormat )
            {
                var f = (PathSingleValueFormat)format;
                var str = Content.GetTextByPath( HtmlPath.Parse( f.Path ) );
                var value = f.ValueFormat.Convert( str );

                // XXX: this is really ugly - i have to create a table just to satisfy the interface :(
                return CreateTableForScalar( f.ValueFormat.Type, value );
            }
            else
            {
                throw new NotSupportedException( "Format not supported for Html documents: " + format.GetType() );
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
