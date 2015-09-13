using System;
using System.Data;
using System.Globalization;
using System.IO;
using Plainion;

namespace RaynMaker.Import.Parsers.Text
{
    class CsvReader
    {
        public static DataTable Read( string file, string separator )
        {
            return CsvReader.Read( file, separator, true );
        }

        public static DataTable Read( string file, string separator, bool skipEmptyRows )
        {
            if( string.IsNullOrEmpty( separator ) )
            {
                throw new ArgumentException( "separator required" );
            }
            DataTable table = new DataTable();
            table.Locale = CultureInfo.InvariantCulture;
            using( StreamReader streamReader = new StreamReader( file ) )
            {
                string[] separator2 = new string[]
				{
					separator
				};
                int num = -1;
                while( !streamReader.EndOfStream )
                {
                    num++;
                    int arg_73_0 = num % 1000;
                    string text = streamReader.ReadLine();
                    if( !skipEmptyRows || !string.IsNullOrEmpty( text ) )
                    {
                        string[] tokens = text.Split( separator2, StringSplitOptions.None );
                        if( tokens.Length > table.Columns.Count )
                        {
                            ( tokens.Length - table.Columns.Count ).Times( delegate( int x )
                            {
                                table.Columns.Add();
                            } );
                        }
                        DataRow row = table.NewRow();
                        tokens.Length.Times( delegate( int i )
                        {
                            row[ i ] = tokens[ i ];
                        } );
                        table.Rows.Add( row );
                    }
                }
            }
            return table;
        }
    }
}
