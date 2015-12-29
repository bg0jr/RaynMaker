using System;
using System.Data;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Parsers.Text
{
    class TextParser : IDocumentParser
    {
        private TextDocument myDocument;
        private IFigureExtractionDescriptor myFormat;

        public TextParser( TextDocument textDocument, IFigureExtractionDescriptor format )
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
                DataTable result = CsvReader.Read( myDocument.Location.LocalPath, csvFormat.Separator );
                return TableFormatter.ToFormattedTable( csvFormat, result );
            }
            else if( separatorSeriesFormat != null )
            {
                DataTable result = CsvReader.Read( myDocument.Location.LocalPath, separatorSeriesFormat.Separator );
                return TableFormatter.ToFormattedTable( separatorSeriesFormat, result );
            }
            else
            {
                throw new NotSupportedException( "Format not supported for text files: " + myFormat.GetType() );
            }
        }
    }
}
