using System;
using System.Data;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Core
{
    internal class TextDocument : IDocument
    {
        internal TextDocument( string file )
        {
            Location = file;
        }

        public string Location { get; private set; }

        public DataTable ExtractTable( IFormat format )
        {
            SeparatorSeriesFormat separatorSeriesFormat = format as SeparatorSeriesFormat;
            CsvFormat csvFormat = format as CsvFormat;

            if ( csvFormat != null )
            {
                DataTable result = CsvReader.Read( Location, csvFormat.Separator );
                return csvFormat.ToFormattedTable( result );
            }
            else if ( separatorSeriesFormat != null )
            {
                DataTable result = CsvReader.Read( Location, separatorSeriesFormat.Separator );
                return separatorSeriesFormat.ToFormattedTable( result );
            }
            else
            {
                throw new NotSupportedException( "Format not supported for text files: " + format.GetType() );
            }
        }
    }
}
