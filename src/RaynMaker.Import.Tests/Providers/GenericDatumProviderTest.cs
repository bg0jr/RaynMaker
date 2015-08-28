using System;
using Blade.Data;
using Blade.IO;
using RaynMaker.Import;
using RaynMaker.Import.Providers;
using NUnit.Framework;
using RaynMaker.Import.Spec;
using System.IO;

namespace RaynMaker.Import.Tests.Providers
{
    // TODO: lets have an additional test for the result validation and with multiple sites
    [TestFixture]
    public class GenericDatumProviderTest : TestBase
    {
        /// <summary>
        /// skip "high", "low" columns intentionally and check whether they are omitted
        /// in the final table
        /// </summary>
        [Test]
        public void ArivaSomePrices()
        {
            DatumLocator locator = new DatumLocator( "StockPrices",
                new Site( "Ariva",
                    new Navigation( DocumentType.Html,
                        Path.Combine( TestDataRoot, "Recognition", "ariva.prices.${stock.isin}.html" ) ),
                    new PathTableFormat( "Ariava.Prices",
                        "/BODY[0]/DIV[5]/DIV[0]/DIV[3]/DIV[0]/TABLE[${TableIndex}]",
                        new FormatColumn( "date", typeof( DateTime ), "dd.MM.yy" ),
                        new FormatColumn( "open", typeof( double ), "000000,0000" ),
                        new FormatColumn( "close", typeof( double ), "000000,0000" ),
                        new FormatColumn( "volume", typeof( int ), "000000,0000" ) )
                        {
                            SkipRows = new int[] { 0, 1 },
                            SkipColumns = new int[] { 0, 3, 4 }
                        },
                    new DataContent( "Euro" ) ) );

            LookupPolicy fetchPolicy = new LookupPolicy();
            fetchPolicy.Lut[ "${stock.isin}" ] = "DE0005003404";
            fetchPolicy.Lut[ "${TableIndex}" ] = "0";

            var browser = DocumentBrowserFactory.Create();
            var provider = new GenericDatumProvider( browser, locator, fetchPolicy, null );

            var result = provider.Fetch();

            Assert.AreEqual( "/BODY[0]/DIV[5]/DIV[0]/DIV[3]/DIV[0]/TABLE[${TableIndex}]", ((PathTableFormat)locator.Sites[ 0 ].Format).Path );

            Assert.IsNotNull( result );
            Assert.IsNotNull( result.ResultTable );

            var table = result.ResultTable;

            Assert.AreEqual( 21, table.Rows.Count );

            Assert.AreEqual( 4, table.Columns.Count );
            Assert.AreEqual( "date", table.Columns[ 0 ].ColumnName );
            Assert.AreEqual( "open", table.Columns[ 1 ].ColumnName );
            Assert.AreEqual( "close", table.Columns[ 2 ].ColumnName );
            Assert.AreEqual( "volume", table.Columns[ 3 ].ColumnName );

            Assert.AreEqual( GetDate( "2008-07-07" ), (DateTime)table.Rows[ 0 ][ "date" ] );
            Assert.AreEqual( 38.37d, (double)table.Rows[ 0 ][ "open" ], 0.000001d );
            Assert.AreEqual( 38.93d, (double)table.Rows[ 0 ][ "close" ], 0.000001d );
            Assert.AreEqual( 1155400, (int)table.Rows[ 0 ][ "volume" ] );

            Assert.AreEqual( GetDate( "2008-06-09" ), (DateTime)table.Rows[ 20 ][ "date" ] );
            Assert.AreEqual( 45.21d, (double)table.Rows[ 20 ][ "open" ], 0.000001d );
            Assert.AreEqual( 44.50d, (double)table.Rows[ 20 ][ "close" ], 0.000001d );
            Assert.AreEqual( 1113865, (int)table.Rows[ 20 ][ "volume" ] );
        }

        [Test]
        public void ArivaEps_DE0005140008()
        {
            var locator = new DatumLocator( "Eps",
                new Site( "Ariva",
                    new Navigation( DocumentType.Html,
                        Path.Combine( TestDataRoot, "Recognition", "ariva.fund.${stock.isin}.html" ) ),
                    new PathSeriesFormat( "Ariava.Eps" )
                    {
                        Path = "/BODY[0]/DIV[5]/DIV[0]/DIV[3]/TABLE[7]/TBODY[0]/TR[5]/TD[1]",
                        TimeAxisPosition = 1,
                        Expand = CellDimension.Row,
                        SeriesNamePosition = 0,
                        ValueFormat = new FormatColumn( "value", typeof( double ), "000000,0000" ),
                        TimeAxisFormat = new FormatColumn( "year", typeof( int ), "0000" )
                    },
                    new DataContent( "Euro" ) ) );

            var fetchPolicy = new LookupPolicy();
            fetchPolicy.Lut[ "${stock.isin}" ] = "DE0005140008";

            var browser = DocumentBrowserFactory.Create();
            var provider = new GenericDatumProvider( browser, locator, fetchPolicy, null );

            var result = provider.Fetch();

            Assert.IsNotNull( result );
            Assert.IsNotNull( result.ResultTable );

            var table = result.ResultTable;

            Assert.AreEqual( 6, table.Rows.Count );

            Assert.AreEqual( 2002, (int)table.Rows[ 0 ][ 1 ] );
            Assert.AreEqual( 0.64d, (double)table.Rows[ 0 ][ 0 ], 0.00001d );
            Assert.AreEqual( 2007, (int)table.Rows[ 5 ][ 1 ] );
            Assert.AreEqual( 13.65d, (double)table.Rows[ 5 ][ 0 ], 0.00001d );
        }
    }
}
