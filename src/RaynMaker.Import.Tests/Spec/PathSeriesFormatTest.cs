using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;
using RaynMaker.Import.Core;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Tests.Spec
{
    [TestFixture]
    public class PathSeriesFormatTest : TestBase
    {
        private PathSeriesFormat myFormat = null;
        private string myPricesFile = null;

        [SetUp]
        public void SetUp()
        {
            myFormat = new PathSeriesFormat( "test" );
            myFormat.Path = @"/BODY[0]/DIV[5]/DIV[0]/DIV[5]/DIV[0]/TABLE[0]/TBODY[0]";
            myFormat.Anchor = Anchor.ForCell( new StringContainsLocator( 1, @"xetra" ), new AbsolutePositionLocator( 2 ) );
            myFormat.TimeAxisPosition = 0;
            myFormat.SeriesNamePosition = 0;

            myPricesFile = Path.Combine( TestDataRoot, "ariva-prices.csv" );
        }

        [Test]
        public void FetchCurrentPrice()
        {
            myFormat.ValueFormat = new FormatColumn( "close", typeof( double ), "000,000", new Regex( @"([\d.,]+)\s*€" ) );
            var table = Parse( myFormat, myPricesFile );

            Assert.AreEqual( 1, table.Rows.Count );
            Assert.AreEqual( 0.18d, ( double )table.Rows[ 0 ][ 0 ], 0.000001d );
        }

        private DataTable Parse( PathSeriesFormat format, string file )
        {
            var rawTable = CsvReader.Read( file, ";" );

            return format.ToFormattedTable( rawTable );
        }
    }
}
