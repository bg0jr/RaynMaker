using System;
using System.Data;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.Parsers.Text
{
    class TextParser : IDocumentParser
    {
        private TextDocument myDocument;
        private IFigureDescriptor myDescriptor;

        public TextParser( TextDocument textDocument, IFigureDescriptor descriptor )
        {
            myDocument = textDocument;
            myDescriptor = descriptor;
        }

        public DataTable ExtractTable()
        {
            SeparatorSeriesDescriptor separatorSeriesDescriptor = myDescriptor as SeparatorSeriesDescriptor;
            CsvDescriptor csvDescriptor = myDescriptor as CsvDescriptor;

            if( csvDescriptor != null )
            {
                DataTable result = CsvReader.Read( myDocument.Location.LocalPath, csvDescriptor.Separator );
                return TableFormatter.ToFormattedTable( csvDescriptor, result );
            }
            else if( separatorSeriesDescriptor != null )
            {
                DataTable result = CsvReader.Read( myDocument.Location.LocalPath, separatorSeriesDescriptor.Separator );
                return TableFormatter.ToFormattedTable( separatorSeriesDescriptor, result );
            }
            else
            {
                throw new NotSupportedException( "Format not supported for text files: " + myDescriptor.GetType() );
            }
        }
    }
}
