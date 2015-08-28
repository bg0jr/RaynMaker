using System;
using System.Globalization;
using System.IO;
using Blade;

namespace RaynMaker.Import.Tests
{
    public class TestBase 
    {
        public TestBase()
        {
            string dir = Path.GetFileNameWithoutExtension( GetType().Assembly.CodeBase );
            dir = dir.Replace( "RaynMaker.Import.", string.Empty );
            dir = dir.Replace( ".Tests", string.Empty );
            TestDataRoot = Path.Combine( GetType().GetAssemblyPath(), "TestData", dir );
        }

        protected string TestDataRoot { get; private set; }

        /// <summary>
        /// Accepted formats:
        /// "2008-07-07"
        /// "2008-07-07 00:00"
        /// </summary>
        protected DateTime GetDate( string s )
        {
            s = s.Trim();

            if( !s.Contains( ":" ) )
            {
                // contains to time -> add it
                s = s += " 00:00";
            }
            return DateTime.Parse( s, CultureInfo.InvariantCulture );
        }
    }
}
