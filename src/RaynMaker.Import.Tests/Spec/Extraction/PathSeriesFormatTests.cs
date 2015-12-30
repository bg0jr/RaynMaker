using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;
using RaynMaker.Import.Parsers;
using RaynMaker.Import.Parsers.Text;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2.Extraction;

namespace RaynMaker.Import.Tests.Spec.Extraction
{
    [TestFixture]
    public class PathSeriesFormatTests : TestBase
    {
        [Test]
        public void FetchCurrentPrice()
        {
            var format = new PathSeriesDescriptor( "test" );
            format.Path = @"/BODY[0]/DIV[5]/DIV[0]/DIV[5]/DIV[0]/TABLE[0]/TBODY[0]";
            format.Anchor = TableFragmentAnchor.ForCell( new StringContainsLocator( 1, @"xetra" ), new AbsolutePositionLocator( 2 ) );
            format.TimeAxisPosition = 0;
            format.SeriesNamePosition = 0;
            format.ValueFormat = new FormatColumn( "close", typeof( double ), "000,000", new Regex( @"([\d.,]+)\s*€" ) );

            var table = Parse( format, Path.Combine( TestDataRoot, "ariva-prices.csv" ) );

            Assert.AreEqual( 1, table.Rows.Count );
            Assert.AreEqual( 0.18d, ( double )table.Rows[ 0 ][ 0 ], 0.000001d );
        }

        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var format = new PathSeriesDescriptor( "dummy" );
            format.Path = "123";
            format.ExtractLinkUrl = true;
            format.SeriesName = "x";

            var clone = FormatFactory.Clone( format );

            Assert.That( clone.Path, Is.EqualTo( "123" ) );
            Assert.That( clone.ExtractLinkUrl, Is.EqualTo( true ) );
            Assert.That( clone.SeriesName, Is.EqualTo( "x" ) );
        }

        private DataTable Parse( PathSeriesDescriptor format, string file )
        {
            var rawTable = CsvReader.Read( file, ";" );

            return TableFormatter.ToFormattedTable( format, rawTable );
        }
    }
}
