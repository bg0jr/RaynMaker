using NUnit.Framework;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.ScenarioTests
{
    [TestFixture]
    public class ReadingFromCsvDocumentScenarios : TestBase
    {
        private IDocument myDocument = null;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            myDocument = LoadDocument<TextDocument>( "DE0005151005.csv" );
        }

        [Test]
        public void GetSeries()
        {
            var descriptor = new SeparatorSeriesDescriptor();
            descriptor.Figure = "EarningsPerShare";
            descriptor.Separator = ";";
            descriptor.Orientation = SeriesOrientation.Row;
            descriptor.ValuesLocator = new StringContainsLocator { HeaderSeriesPosition = 0, Pattern = "EPS" };
            descriptor.TimesLocator = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 0 };
            descriptor.Excludes.AddRange( 0, 1 );
            descriptor.ValueFormat = new FormatColumn( "value", typeof( double ), "000,000" );
            descriptor.TimeFormat = new FormatColumn( "year", typeof( int ), "000" );

            var parser = DocumentProcessorsFactory.CreateParser( myDocument, descriptor );
            var table = parser.ExtractTable();

            Assert.AreEqual( 10, table.Rows.Count );
            Assert.AreEqual( 3.2d, ( double )table.Rows[ 0 ][ "value" ], 0.000001d );
            Assert.AreEqual( 3.4d, ( double )table.Rows[ 1 ][ "value" ], 0.000001d );
            Assert.AreEqual( 3.4d, ( double )table.Rows[ 2 ][ "value" ], 0.000001d );
            Assert.AreEqual( 3.3d, ( double )table.Rows[ 3 ][ "value" ], 0.000001d );
            Assert.AreEqual( 2.9d, ( double )table.Rows[ 4 ][ "value" ], 0.000001d );
            Assert.AreEqual( 2.8d, ( double )table.Rows[ 5 ][ "value" ], 0.000001d );
            Assert.AreEqual( 3.0d, ( double )table.Rows[ 6 ][ "value" ], 0.000001d );
            Assert.AreEqual( 3.0d, ( double )table.Rows[ 7 ][ "value" ], 0.000001d );
            Assert.AreEqual( 3.1d, ( double )table.Rows[ 8 ][ "value" ], 0.000001d );
            Assert.AreEqual( 3.5d, ( double )table.Rows[ 9 ][ "value" ], 0.000001d );

            Assert.AreEqual( 1997, ( int )table.Rows[ 0 ][ "year" ] );
            Assert.AreEqual( 1998, ( int )table.Rows[ 1 ][ "year" ] );
            Assert.AreEqual( 1999, ( int )table.Rows[ 2 ][ "year" ] );
            Assert.AreEqual( 2000, ( int )table.Rows[ 3 ][ "year" ] );
            Assert.AreEqual( 2001, ( int )table.Rows[ 4 ][ "year" ] );
            Assert.AreEqual( 2002, ( int )table.Rows[ 5 ][ "year" ] );
            Assert.AreEqual( 2003, ( int )table.Rows[ 6 ][ "year" ] );
            Assert.AreEqual( 2004, ( int )table.Rows[ 7 ][ "year" ] );
            Assert.AreEqual( 2005, ( int )table.Rows[ 8 ][ "year" ] );
            Assert.AreEqual( 2006, ( int )table.Rows[ 9 ][ "year" ] );
        }
    }
}
