using System;
using System.Data;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2.Extraction;

namespace RaynMaker.Import.Parsers.Text
{
    class TextParser : IDocumentParser
    {
        private TextDocument myDocument;
        private IFigureDescriptor myFormat;

        public TextParser( TextDocument textDocument, IFigureDescriptor format )
        {
            myDocument = textDocument;
            myFormat = format;
        }

        public DataTable ExtractTable()
        {
            SeparatorSeriesDescriptor separatorSeriesFormat = myFormat as SeparatorSeriesDescriptor;
            CsvDescriptor csvFormat = myFormat as CsvDescriptor;

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
