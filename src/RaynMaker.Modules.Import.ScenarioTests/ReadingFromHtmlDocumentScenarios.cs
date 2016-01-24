using System;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using RaynMaker.Entities;
using RaynMaker.Entities.Figures;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.ScenarioTests
{
    [TestFixture]
    [RequiresSTA]
    public class ReadingFromHtmlDocumentScenarios : TestBase
    {
        [Test]
        public void GetSingleValue()
        {
            var doc = LoadDocument<IHtmlDocument>( "Html", "ariva.overview.US0138171014.html" );

            var descriptor = new PathSingleValueDescriptor();
            descriptor.Path = @"/BODY[0]/DIV[4]/DIV[0]/DIV[3]/DIV[0]";
            descriptor.ValueFormat = new ValueFormat( typeof( int ), "00000000" ) { ExtractionPattern = new Regex( @"WKN: (\d+)" ) };

            var parser = DocumentProcessingFactory.CreateParser( doc, descriptor );
            var table = parser.ExtractTable();

            Assert.AreEqual( 1, table.Rows.Count );

            Assert.AreEqual( 850206, table.Rows[ 0 ][ 0 ] );
        }

        [Test]
        public void GetSeriesAndConvertToEntities()
        {
            var doc = LoadDocument<IHtmlDocument>( "Html", "ariva.fundamentals.DE0005190003.html" );

            var dataSource = new DataSource();
            dataSource.Vendor = "Ariva";
            dataSource.Name = "Fundamentals";
            dataSource.Quality = 1;

            var descriptor = new PathSeriesDescriptor();
            descriptor.Figure = "Dividend";
            descriptor.Path = @"/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]";
            descriptor.Orientation = SeriesOrientation.Row;
            descriptor.ValuesLocator = new StringContainsLocator { HeaderSeriesPosition = 0, Pattern = "Dividendenausschüttung" };
            descriptor.ValueFormat = new FormatColumn( "value", typeof( double ), "00,00" ) { InMillions = true };
            descriptor.TimesLocator = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 1 };
            descriptor.TimeFormat = new FormatColumn( "year", typeof( int ), "00000000" );
            descriptor.Excludes.Add( 0 );

            var parser = DocumentProcessingFactory.CreateParser( doc, descriptor );
            var table = parser.ExtractTable();

            Assert.AreEqual( 6, table.Rows.Count );

            Assert.AreEqual( 350000000d, table.Rows[ 0 ][ 0 ] );
            Assert.AreEqual( 351000000d, table.Rows[ 1 ][ 0 ] );
            Assert.AreEqual( 392000000d, table.Rows[ 2 ][ 0 ] );
            Assert.AreEqual( 419000000d, table.Rows[ 3 ][ 0 ] );
            Assert.AreEqual( 424000000d, table.Rows[ 4 ][ 0 ] );
            Assert.AreEqual( 458000000d, table.Rows[ 5 ][ 0 ] );

            Assert.AreEqual( 2001, table.Rows[ 0 ][ 1 ] );
            Assert.AreEqual( 2002, table.Rows[ 1 ][ 1 ] );
            Assert.AreEqual( 2003, table.Rows[ 2 ][ 1 ] );
            Assert.AreEqual( 2004, table.Rows[ 3 ][ 1 ] );
            Assert.AreEqual( 2005, table.Rows[ 4 ][ 1 ] );
            Assert.AreEqual( 2006, table.Rows[ 5 ][ 1 ] );

            var stock = new Stock { Isin = "DE0007664039" };
            stock.Company = new Company { Name = "Volkswagen" };
            stock.Company.Stocks.Add( stock );

            var converter = DocumentProcessingFactory.CreateConverter( descriptor, dataSource, Enumerable.Empty<Currency>() );
            var series = converter.Convert( table, stock ).Cast<Dividend>().ToList();

            foreach ( var dividend in series )
            {
                Assert.That( dividend.Company.Stocks.First().Isin, Is.EqualTo( "DE0007664039" ) );
                Assert.That( dividend.Period, Is.InstanceOf<YearPeriod>() );
                Assert.That( dividend.Source, Is.StringContaining( "ariva" ).IgnoreCase.And.StringContaining( "fundamentals" ).IgnoreCase );
                Assert.That( dividend.Timestamp.Date, Is.EqualTo( DateTime.Today ) );

                // Descriptor does not provide static currency
                Assert.That( dividend.Currency, Is.Null );
            }

            Assert.That( series[ 0 ].Period, Is.EqualTo( new YearPeriod( 2001 ) ) );
            Assert.That( series[ 0 ].Value, Is.EqualTo( 350000000d ) );
            Assert.That( series[ 1 ].Period, Is.EqualTo( new YearPeriod( 2002 ) ) );
            Assert.That( series[ 1 ].Value, Is.EqualTo( 351000000d ) );
            Assert.That( series[ 2 ].Period, Is.EqualTo( new YearPeriod( 2003 ) ) );
            Assert.That( series[ 2 ].Value, Is.EqualTo( 392000000d ) );
            Assert.That( series[ 3 ].Period, Is.EqualTo( new YearPeriod( 2004 ) ) );
            Assert.That( series[ 3 ].Value, Is.EqualTo( 419000000d ) );
            Assert.That( series[ 4 ].Period, Is.EqualTo( new YearPeriod( 2005 ) ) );
            Assert.That( series[ 4 ].Value, Is.EqualTo( 424000000d ) );
            Assert.That( series[ 5 ].Period, Is.EqualTo( new YearPeriod( 2006 ) ) );
            Assert.That( series[ 5 ].Value, Is.EqualTo( 458000000d ) );
        }

        [Test]
        public void GetCellAndConvertToEntity()
        {
            var doc = LoadDocument<IHtmlDocument>( "Html", "ariva.prices.DE0007664039.html" );

            var dataSource = new DataSource();
            dataSource.Vendor = "Ariva";
            dataSource.Name = "Prices";
            dataSource.Quality = 1;

            var descriptor = new PathCellDescriptor();
            descriptor.Figure = "Price";
            descriptor.Path = @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]/TBODY[0]";
            descriptor.Column = new StringContainsLocator { HeaderSeriesPosition = 0, Pattern = "Letzter" };
            descriptor.Row = new StringContainsLocator { HeaderSeriesPosition = 0, Pattern = "Frankfurt" };
            descriptor.ValueFormat = new FormatColumn( "value", typeof( double ), "00,00" ) { ExtractionPattern = new Regex( @"([0-9,\.]+)" ) };
            descriptor.Currency = "EUR";

            var parser = DocumentProcessingFactory.CreateParser( doc, descriptor );
            var table = parser.ExtractTable();

            Assert.That( table.Rows.Count, Is.EqualTo( 1 ) );

            var value = table.Rows[ 0 ][ 0 ];

            Assert.That( value, Is.EqualTo( 134.356d ) );

            var converter = DocumentProcessingFactory.CreateConverter( descriptor, dataSource, new[] { new Currency { Symbol = "EUR" } } );
            var series = converter.Convert( table, new Stock { Isin = "DE0007664039" } );

            var price = (Price)series.Single();

            Assert.That( price.Stock.Isin, Is.EqualTo( "DE0007664039" ) );
            Assert.That( ( (DayPeriod)price.Period ).Day.Date, Is.EqualTo( DateTime.Today ) );
            Assert.That( price.Source, Is.StringContaining( "ariva" ).IgnoreCase.And.StringContaining( "price" ).IgnoreCase );
            Assert.That( price.Timestamp.Date, Is.EqualTo( DateTime.Today ) );
            Assert.That( price.Value, Is.EqualTo( 134.356d ) );
            Assert.That( price.Currency.Symbol, Is.EqualTo( "EUR" ) );
        }

        [Test]
        public void GetTable()
        {
            var doc = LoadDocument<IHtmlDocument>( "Html", "ariva.historical.prices.DE0007664039.html" );

            var descriptor = new PathTableDescriptor();
            descriptor.Figure = "HistoricalPrices";
            descriptor.Path = @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]/TBODY[0]";
            descriptor.Columns.Add( new FormatColumn( "date", typeof( DateTime ), "dd.MM.yy" ) );
            descriptor.Columns.Add( new FormatColumn( "open", typeof( double ), "00,00" ) );
            descriptor.Columns.Add( new FormatColumn( "high", typeof( double ), "00,00" ) );
            descriptor.Columns.Add( new FormatColumn( "low", typeof( double ), "00,00" ) );
            descriptor.Columns.Add( new FormatColumn( "close", typeof( double ), "00,00" ) );
            descriptor.SkipColumns.AddRange( 5, 6 );
            descriptor.SkipRows.AddRange( 0, 23 );

            var parser = DocumentProcessingFactory.CreateParser( doc, descriptor );
            var table = parser.ExtractTable();

            Assert.That( table.Rows.Count, Is.EqualTo( 22 ) );

            Assert.That( table.Rows[ 0 ][ 0 ], Is.EqualTo( new DateTime( 2015, 12, 30 ) ) );
            Assert.That( table.Rows[ 0 ][ 1 ], Is.EqualTo( 135.45d ) );
            Assert.That( table.Rows[ 0 ][ 2 ], Is.EqualTo( 135.45d ) );
            Assert.That( table.Rows[ 0 ][ 3 ], Is.EqualTo( 133.55d ) );
            Assert.That( table.Rows[ 0 ][ 4 ], Is.EqualTo( 133.75d ) );

            Assert.That( table.Rows[ 21 ][ 0 ], Is.EqualTo( new DateTime( 2015, 11, 27 ) ) );
            Assert.That( table.Rows[ 21 ][ 1 ], Is.EqualTo( 124.50d ) );
            Assert.That( table.Rows[ 21 ][ 2 ], Is.EqualTo( 125.10d ) );
            Assert.That( table.Rows[ 21 ][ 3 ], Is.EqualTo( 121.05d ) );
            Assert.That( table.Rows[ 21 ][ 4 ], Is.EqualTo( 123.85d ) );
        }
    }
}
