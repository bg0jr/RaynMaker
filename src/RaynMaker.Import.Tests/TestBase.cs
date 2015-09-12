using System;
using System.Globalization;
using System.IO;
using Blade;
using NUnit.Framework;

namespace RaynMaker.Import.Tests
{
    public class TestBase 
    {
        public TestBase()
        {
            var assemblyPath = new Uri( Path.GetDirectoryName( GetType().Assembly.CodeBase ) ).LocalPath;
            TestDataRoot = Path.Combine( assemblyPath, "TestData" );
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
