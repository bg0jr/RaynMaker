using System.Data;
using System.IO;
using NUnit.Framework;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Parsers;
using RaynMaker.Import.Parsers.Text;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2.Extraction;

namespace RaynMaker.Import.Tests.Spec.Extraction
{
    [TestFixture]
    public class SeparatorSeriesFormatTests : TestBase
    {
        private SeparatorSeriesDescriptor myFormat = null;
        private string myEpsFile = null;

        [SetUp]
        public void SetUp()
        {
            myFormat = new SeparatorSeriesDescriptor( "test" );
            myFormat.Separator = ";";
            myFormat.Anchor = TableFragmentAnchor.ForRow( new StringContainsLocator( 1, "DE0005151005" ) );
            myFormat.Expand = CellDimension.Row;
            myFormat.TimeAxisPosition = 0;
            myFormat.SeriesNamePosition = 1;
            myFormat.SkipColumns = new int[] { 0, 2, 3 };
            myFormat.SkipRows = new int[] { 1 };

            myEpsFile = Path.Combine( TestDataRoot, "eps.csv" );
        }

        [Test]
        public void FetchSeries()
        {
            var table = Parse( myFormat, myEpsFile );

            Assert.AreEqual( 10, table.Rows.Count );
            Assert.AreEqual( "3,2", table.Rows[ 0 ][ 0 ] );
            Assert.AreEqual( "3,4", table.Rows[ 1 ][ 0 ] );
            Assert.AreEqual( "3,4", table.Rows[ 2 ][ 0 ] );
            Assert.AreEqual( "3,3", table.Rows[ 3 ][ 0 ] );
            Assert.AreEqual( "2,9", table.Rows[ 4 ][ 0 ] );
            Assert.AreEqual( "2,8", table.Rows[ 5 ][ 0 ] );
            Assert.AreEqual( "3", table.Rows[ 6 ][ 0 ] );
            Assert.AreEqual( "3", table.Rows[ 7 ][ 0 ] );
            Assert.AreEqual( "3,1", table.Rows[ 8 ][ 0 ] );
            Assert.AreEqual( "3,5", table.Rows[ 9 ][ 0 ] );

            Assert.AreEqual( "1997", table.Rows[ 0 ][ 1 ] );
            Assert.AreEqual( "1998", table.Rows[ 1 ][ 1 ] );
            Assert.AreEqual( "1999", table.Rows[ 2 ][ 1 ] );
            Assert.AreEqual( "2000", table.Rows[ 3 ][ 1 ] );
            Assert.AreEqual( "2001", table.Rows[ 4 ][ 1 ] );
            Assert.AreEqual( "2002", table.Rows[ 5 ][ 1 ] );
            Assert.AreEqual( "2003", table.Rows[ 6 ][ 1 ] );
            Assert.AreEqual( "2004", table.Rows[ 7 ][ 1 ] );
            Assert.AreEqual( "2005", table.Rows[ 8 ][ 1 ] );
            Assert.AreEqual( "2006", table.Rows[ 9 ][ 1 ] );
        }

        [Test]
        public void FetchTypedSeries()
        {
            myFormat.ValueFormat = new FormatColumn( "value", typeof( double ), "000,000" );
            myFormat.TimeAxisFormat = new FormatColumn( "year", typeof( int ), "000" );

            var table = Parse( myFormat, myEpsFile );

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
        public void FetchTypedSeriesWithoutTimeaxis()
        {
            myFormat.ValueFormat = new FormatColumn( "value", typeof( double ), "000,000" );

            var table = Parse( myFormat, myEpsFile );

            // no time axis
            Assert.AreEqual( 1, table.Columns.Count );

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
        }

        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var format = new SeparatorSeriesDescriptor( "dummy" );
            format.Separator = "#";

            var clone = FormatFactory.Clone( format );

            Assert.That( clone.Separator, Is.EqualTo( "#" ) );
        }

        private DataTable Parse( SeparatorSeriesDescriptor format, string file )
        {
            var rawTable = CsvReader.Read( file, format.Separator );
            return TableFormatter.ToFormattedTable( format, rawTable );
        }
    }
}
