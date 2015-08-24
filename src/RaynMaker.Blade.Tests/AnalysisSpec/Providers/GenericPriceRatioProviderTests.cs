using System;
using System.ComponentModel;
using System.Linq;
using Moq;
using NUnit.Framework;
using RaynMaker.Blade.AnalysisSpec.Providers;
using RaynMaker.Blade.Engine;
using RaynMaker.Blade.Entities;
using RaynMaker.Blade.Entities.Datums;
using RaynMaker.Blade.Tests.Fakes;
using RaynMaker.Entities;

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
            myContext.Setup( x => x.Stock ).Returns( () =>
                {
                    var asset = new RaynMaker.Blade.Entities.Stock();
                    if( myCurrentPrice != null )
                    {
                        asset.Data.Add( new DatumSeries( typeof( Price ), myCurrentPrice ) );
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
            myRhsSeries = new DatumSeries( typeof( FakeDatum ), DatumFactory.New( 2015, 1 ) );

            var result = myProvider.ProvideValue( myContext.Object );

            Assert.That( result, Is.InstanceOf<MissingData>() );
            Assert.That( ( ( MissingData )result ).Datum, Is.EqualTo( "Price" ) );
        }

        [Test]
        public void ProvideValue_RhsSeriesEmpty_ReturnsMissingData()
        {
            myCurrentPrice = DatumFactory.NewPrice( "2015-01-01", 17.21, Euro );
            myRhsSeries = DatumSeries.Empty;

            var result = myProvider.ProvideValue( myContext.Object );

            Assert.That( result, Is.InstanceOf<MissingData>() );
            Assert.That( ( ( MissingData )result ).Datum, Is.EqualTo( RhsSeriesName ) );
        }

        [Test]
        public void ProvideValue_PriceWithoutCurrency_Throws()
        {
            myCurrentPrice = DatumFactory.NewPrice( "2015-01-01", 17.21, null );
            myRhsSeries = new DatumSeries( typeof( FakeDatum ), DatumFactory.New( 2015, 1 ) );

            var ex = Assert.Throws<ArgumentException>( () => myProvider.ProvideValue( myContext.Object ) );
            Assert.That( ex.Message, Is.StringContaining( "Currency missing" ) );
        }

        [Test]
        public void ProvideValue_RhsHasNoDataForPeriod_MissingDataForPeriodReturned()
        {
            myCurrentPrice = DatumFactory.NewPrice( "2015-01-01", 17.21, Euro );
            myRhsSeries = new DatumSeries( typeof( FakeDatum ), DatumFactory.New( 2001, 1 ) );

            var result = myProvider.ProvideValue( myContext.Object );

            Assert.That( result, Is.InstanceOf<MissingDataForPeriod>() );
            Assert.That( ( ( MissingDataForPeriod )result ).Datum, Is.EqualTo( RhsSeriesName ) );
        }

        [Test]
        public void ProvideValue_WithValidInputData_RatioReturned()
        {
            myCurrentPrice = DatumFactory.NewPrice( "2015-01-01", 17.21, Euro );
            myRhsSeries = new DatumSeries( typeof( FakeDatum ), DatumFactory.New( 2015, 23 ), DatumFactory.New( 2014, 37 ) );

            var result = ( ICurrencyDatum )myProvider.ProvideValue( myContext.Object );

            Assert.That( result.Period, Is.EqualTo( myCurrentPrice.Period ) );
            Assert.That( result.Value, Is.EqualTo( 17.21 + 23 ) );
            Assert.That( result.Currency, Is.EqualTo( myCurrentPrice.Currency ) );
        }

        [Test]
        public void ProvideValue_WhenCalled_InputsReferenced()
        {
            myCurrentPrice = DatumFactory.NewPrice( "2015-01-01", 17.21, Euro );
            myRhsSeries = new DatumSeries( typeof( FakeCurrencyDatum ), DatumFactory.New( 2015, 23, Euro ), DatumFactory.New( 2014, 37, Euro ) );

            var result = ( DerivedDatum )myProvider.ProvideValue( myContext.Object );

            Assert.That( result.Inputs, Is.EquivalentTo( new[] { myCurrentPrice, myRhsSeries.ElementAt( 1 ) } ) );
        }


        [Test]
        public void ProvideValue_InconsistentCurrencies_Throws()
        {
            myCurrentPrice = DatumFactory.NewPrice( "2015-01-01", 17.21, Euro );
            myRhsSeries = new DatumSeries( typeof( FakeCurrencyDatum ), DatumFactory.New( 2015, 23, Dollar ), DatumFactory.New( 2014, 37, Dollar ) );

            var ex = Assert.Throws<ArgumentException>( () => myProvider.ProvideValue( myContext.Object ) );
            Assert.That( ex.Message, Is.StringContaining( "Currency inconsistencies" ) );
        }
    }
}
