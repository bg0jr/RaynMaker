
using System.IO;
using RaynMaker.Blade.DataSheetSpec;
namespace RaynMaker.Blade.Engine
{
    public class Report
    {
        private TextWriter myWriter;

        public Report( TextWriter writer )
        {
            myWriter = writer;
        }

        public void Write( string key, string value )
        {
            myWriter.WriteLine( "{0}: {1}", key, value );
        }

        public void Write( string key, Price value ) 
        {
            myWriter.WriteLine( "{0}: {1} {2} ({3})", key, value.Value, value.Curreny.Name, value.Date.ToShortDateString() );
        }
    }
}
