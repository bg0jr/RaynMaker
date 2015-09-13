using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaynMaker.Import.Core;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Parsers
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
