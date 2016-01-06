using System;
using NUnit.Framework;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.ScenarioTests
{
    [TestFixture]
    public class ReadingFromCsvDocumentScenarios : TestBase
    {
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

            var doc = LoadDocument<TextDocument>( "Csv", "DE0005151005.csv" );
            var parser = DocumentProcessingFactory.CreateParser( doc, descriptor );
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

        [Test]
        public void GetTable()
        {
            var descriptor = new CsvDescriptor();
            descriptor.Figure = "HistoricalPrices";
            descriptor.Separator = ";";
            descriptor.SkipColumns.Add( 1 );
            descriptor.SkipRows.Add( 0 );
            descriptor.Columns.Add( new FormatColumn( "Date", typeof( DateTime ) ) );
            descriptor.Columns.Add( new FormatColumn( "High", typeof( double ), "000,000.00" ) );
            descriptor.Columns.Add( new FormatColumn( "Low", typeof( double ), "000,000.00" ) );
            descriptor.Columns.Add( new FormatColumn( "Open", typeof( double ), "000,000.00" ) );
            descriptor.Columns.Add( new FormatColumn( "Close", typeof( double ), "000,000.00" ) );

            var doc = LoadDocument<TextDocument>( "Csv", "Prices.csv" );
            var parser = DocumentProcessingFactory.CreateParser( doc, descriptor );
            var table = parser.ExtractTable();

            Assert.That( table.Rows.Count, Is.EqualTo( 3 ) );
            Assert.That( table.Rows[ 0 ][ "Date" ], Is.EqualTo( DateTime.Parse( "01.01.2016" ) ) );
        }
    }
}
