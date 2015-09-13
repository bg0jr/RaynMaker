using System;
using System.Data;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Parsers.Text
{
    class TextParser : IDocumentParser
    {
        private TextDocument myDocument;
        private IFormat myFormat;

        public TextParser( TextDocument textDocument, IFormat format )
        {
            myDocument = textDocument;
            myFormat = format;
        }

        public DataTable ExtractTable()
        {
            SeparatorSeriesFormat separatorSeriesFormat = myFormat as SeparatorSeriesFormat;
            CsvFormat csvFormat = myFormat as CsvFormat;

            if( csvFormat != null )
            {
                DataTable result = CsvReader.Read( myDocument.Location, csvFormat.Separator );
                return csvFormat.ToFormattedTable( result );
            }
            else if( separatorSeriesFormat != null )
            {
                DataTable result = CsvReader.Read( myDocument.Location, separatorSeriesFormat.Separator );
                return separatorSeriesFormat.ToFormattedTable( result );
            }
            else
            {
                throw new NotSupportedException( "Format not supported for text files: " + myFormat.GetType() );
            }
        }
    }
}
