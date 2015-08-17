using System;
using System.ComponentModel;
using System.Linq;
using Moq;
using NUnit.Framework;
using RaynMaker.Blade.AnalysisSpec.Providers;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.DataSheetSpec.Datums;
using RaynMaker.Blade.Engine;
using RaynMaker.Blade.Entities;

namespace RaynMaker.Blade.Tests.AnalysisSpec.Providers
{
    [TestFixture]
    public class GenericPriceRatioProviderTests
    {
        private const string RhsSeriesName = "S2";

        private static readonly Currency Euro = new Currency { Name = "Euro" };
        private static readonly Currency Dollar = new Currency { Name = "Dollar" };

        private Mock<IFigureProviderContext> myContext;
        private GenericPriceRatioProvider myProvider;
        private Price myCurrentPrice;
        private IDatumSeries myRhsSeries;

        [SetUp]
        public void SetUp()
        {
            myContext = new Mock<IFigureProviderContext> { DefaultValue = DefaultValue.Mock };
            myContext.Setup( x => x.Asset ).Returns( () =>
                {
                    var asset = new Stock();
                    if( myCurrentPrice != null )
                    {
                        asset.Data.Add( new Series( myCurrentPrice ) );
                    }
                    return asset;
                } );
            myContext.Setup( x => x.GetSeries( RhsSeriesName ) ).Returns( () => myRhsSeries );

            myProvider = new GenericPriceRatioProvider( "dummy", RhsSeriesName, ( lhs, rhs ) => lhs + rhs );
        }

        [TearDown]
        public void TearDown()
        {
            myContext = null;
            myProvider = null;
            myCurrentPrice = null;
            myRhsSeries = null;
        }

        [Test]
        public void ProvideValue_PriceMissing_ReturnsMissingData()
        {
            myCurrentPrice = null;
            myRhsSeries = new Series( CreateDatum( 2015, 1 ) );
            myRhsSeries.Freeze();

            var result = myProvider.ProvideValue( myContext.Object );

            Assert.That( result, Is.InstanceOf<MissingData>() );
            Assert.That( ( ( MissingData )result ).Datum, Is.EqualTo( "Price" ) );
        }

        [Test]
        public void ProvideValue_RhsSeriesEmpty_ReturnsMissingData()
        {
            myCurrentPrice = CreatePrice( "2015-01-01", 17.21, Euro );
            myRhsSeries = Series.Empty;

            var result = myProvider.ProvideValue( myContext.Object );

            Assert.That( result, Is.InstanceOf<MissingData>() );
            Assert.That( ( ( MissingData )result ).Datum, Is.EqualTo( RhsSeriesName ) );
        }

        [Test]
        public void ProvideValue_PriceWithoutCurrency_Throws()
        {
            myCurrentPrice = CreatePrice( "2015-01-01", 17.21, null );
            myRhsSeries = new Series( CreateDatum( 2015, 1 ) );
            myRhsSeries.Freeze();

            var ex = Assert.Throws<ArgumentException>( () => myProvider.ProvideValue( myContext.Object ) );
            Assert.That( ex.Message, Is.StringContaining( "Currency missing" ) );
        }

        [Test]
        public void ProvideValue_RhsNotFrozen_Throws()
        {
            myCurrentPrice = CreatePrice( "2015-01-01", 17.21, Euro );
            myRhsSeries = new Series( CreateDatum( 2015, 1 ) );

            var ex = Assert.Throws<ArgumentException>( () => myProvider.ProvideValue( myContext.Object ) );
            Assert.That( ex.Message, Is.StringContaining( "not frozen" ).And.StringContaining( RhsSeriesName ) );
        }

        [Test]
        public void ProvideValue_RhsHasNoDataForPeriod_MissingDataForPeriodReturned()
        {
            myCurrentPrice = CreatePrice( "2015-01-01", 17.21, Euro );
            myRhsSeries = new Series( CreateDatum( 2001, 1 ) );
            myRhsSeries.Freeze();

            var result = myProvider.ProvideValue( myContext.Object );

            Assert.That( result, Is.InstanceOf<MissingDataForPeriod>() );
            Assert.That( ( ( MissingDataForPeriod )result ).Datum, Is.EqualTo( RhsSeriesName ) );
        }

        [Test]
        public void ProvideValue_WithValidInputData_RatioReturned()
        {
            myCurrentPrice = CreatePrice( "2015-01-01", 17.21, Euro );
            myRhsSeries = new Series( CreateDatum( 2015, 23 ), CreateDatum( 2014, 37 ) );
            myRhsSeries.Freeze();

            var result = ( ICurrencyDatum )myProvider.ProvideValue( myContext.Object );

            Assert.That( result.Period, Is.EqualTo( myCurrentPrice.Period ) );
            Assert.That( result.Value, Is.EqualTo( 17.21 + 23 ) );
            Assert.That( result.Currency, Is.EqualTo( myCurrentPrice.Currency ) );
        }

        [Test]
        public void ProvideValue_WhenCalled_InputsReferenced()
        {
            myCurrentPrice = CreatePrice( "2015-01-01", 17.21, Euro );
            myRhsSeries = new Series( CreateDatum( 2015, 23, Euro ), CreateDatum( 2014, 37, Euro ) );
            myRhsSeries.Freeze();

            var result = ( DerivedDatum )myProvider.ProvideValue( myContext.Object );

            Assert.That( result.Inputs, Is.EquivalentTo( new[] { myCurrentPrice, myRhsSeries.First() } ) );
        }


        [Test]
        public void ProvideValue_InconsistentCurrencies_Throws()
        {
            myCurrentPrice = CreatePrice( "2015-01-01", 17.21, Euro );
            myRhsSeries = new Series( CreateDatum( 2015, 23, Dollar ), CreateDatum( 2014, 37, Dollar ) );
            myRhsSeries.Freeze();

            var ex = Assert.Throws<ArgumentException>( () => myProvider.ProvideValue( myContext.Object ) );
            Assert.That( ex.Message, Is.StringContaining( "Currency inconsistencies" ) );
        }

        private IDatum CreateDatum( int year, double value )
        {
            return new Datum
            {
                Period = new YearPeriod( year ),
                Value = value,
                Source = "Dummy",
                Timestamp = DateTime.UtcNow
            };
        }

        private IDatum CreateDatum( int year, double value, Currency currency )
        {
            return new CurrencyDatum
            {
                Period = new YearPeriod( year ),
                Value = value,
                Currency = currency,
                Source = "Dummy",
                Timestamp = DateTime.UtcNow
            };
        }

        private Price CreatePrice( string day, double price, Currency currency )
        {
            var converter = new DateTimeConverter();
            return new Price
            {
                Period = new DayPeriod( ( DateTime )converter.ConvertFrom( day ) ),
                Value = price,
                Currency = currency,
                Source = "Dummy",
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
