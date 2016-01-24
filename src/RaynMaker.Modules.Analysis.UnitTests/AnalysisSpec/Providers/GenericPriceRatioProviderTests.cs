using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using RaynMaker.Modules.Analysis.AnalysisSpec.Providers;
using RaynMaker.Modules.Analysis.Engine;
using RaynMaker.Entities;
using RaynMaker.Entities.Figures;
using RaynMaker.Entities.UnitTests.Fakes;

namespace RaynMaker.Modules.Analysis.UnitTests.AnalysisSpec.Providers
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
        private IFigureSeries myRhsSeries;

        [SetUp]
        public void SetUp()
        {
            myContext = new Mock<IFigureProviderContext> { DefaultValue = DefaultValue.Mock };
            myContext.Setup( x => x.Stock ).Returns( () => new Stock() );
            myContext.Setup( x => x.Data ).Returns( () =>
            {
                var data = new List<IFigureSeries>();
                if( myCurrentPrice != null )
                {
                    data.Add( new FigureSeries( typeof( Price ), myCurrentPrice ) );
                }
                return data;
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
            myRhsSeries = new FigureSeries( typeof( FakeFigure ), FigureFactory.New( 2015, 1 ) );

            var result = myProvider.ProvideValue( myContext.Object );

            Assert.That( result, Is.InstanceOf<MissingData>() );
            Assert.That( ( ( MissingData )result ).Figure, Is.EqualTo( "Price" ) );
        }

        [Test]
        public void ProvideValue_RhsSeriesEmpty_ReturnsMissingData()
        {
            myCurrentPrice = FigureFactory.NewPrice( "2015-01-01", 17.21, Euro );
            myRhsSeries = FigureSeries.Empty;

            var result = myProvider.ProvideValue( myContext.Object );

            Assert.That( result, Is.InstanceOf<MissingData>() );
            Assert.That( ( ( MissingData )result ).Figure, Is.EqualTo( RhsSeriesName ) );
        }

        [Test]
        public void ProvideValue_PriceWithoutCurrency_Throws()
        {
            myCurrentPrice = FigureFactory.NewPrice( "2015-01-01", 17.21, null );
            myRhsSeries = new FigureSeries( typeof( FakeFigure ), FigureFactory.New( 2015, 1 ) );

            var ex = Assert.Throws<ArgumentException>( () => myProvider.ProvideValue( myContext.Object ) );
            Assert.That( ex.Message, Is.StringContaining( "Currency missing" ) );
        }

        [Test]
        public void ProvideValue_RhsHasNoDataForPeriod_MissingDataForPeriodReturned()
        {
            myCurrentPrice = FigureFactory.NewPrice( "2015-01-01", 17.21, Euro );
            myRhsSeries = new FigureSeries( typeof( FakeCurrencyFigure ), FigureFactory.New( 2001, 1, Euro ) );

            var result = myProvider.ProvideValue( myContext.Object );

            Assert.That( result, Is.InstanceOf<MissingDataForPeriod>() );
            Assert.That( ( ( MissingDataForPeriod )result ).Figure, Is.EqualTo( RhsSeriesName ) );
        }

        [Test]
        public void ProvideValue_WithValidInputData_RatioReturned()
        {
            myCurrentPrice = FigureFactory.NewPrice( "2015-01-01", 17.21, Euro );
            myRhsSeries = new FigureSeries( typeof( FakeCurrencyFigure ), FigureFactory.New( 2015, 23, Euro ), FigureFactory.New( 2014, 37, Euro ) );

            var result = ( ICurrencyFigure )myProvider.ProvideValue( myContext.Object );

            Assert.That( result.Period, Is.EqualTo( myCurrentPrice.Period ) );
            Assert.That( result.Value, Is.EqualTo( 17.21 + 23 ) );
            Assert.That( result.Currency, Is.EqualTo( myCurrentPrice.Currency ) );
        }

        [Test]
        public void ProvideValue_WhenCalled_InputsReferenced()
        {
            myCurrentPrice = FigureFactory.NewPrice( "2015-01-01", 17.21, Euro );
            myRhsSeries = new FigureSeries( typeof( FakeCurrencyFigure ), FigureFactory.New( 2015, 23, Euro ), FigureFactory.New( 2014, 37, Euro ) );

            var result = ( DerivedFigure )myProvider.ProvideValue( myContext.Object );

            Assert.That( result.Inputs, Is.EquivalentTo( new[] { myCurrentPrice, myRhsSeries.ElementAt( 1 ) } ) );
        }

        [Test]
        public void ProvideValue_InconsistentCurrencies_PriceCurrencyTranslated()
        {
            myCurrentPrice = FigureFactory.NewPrice( "2015-01-01", 17.21, Euro );
            myRhsSeries = new FigureSeries( typeof( FakeCurrencyFigure ), FigureFactory.New( 2015, 23, Dollar ), FigureFactory.New( 2014, 37, Dollar ) );
            myContext.Setup( x => x.TranslateCurrency( It.IsAny<double>(), It.IsAny<Currency>(), It.IsAny<Currency>() ) )
                .Returns<double, Currency, Currency>( ( value, source, target ) => value * 2 );

            var result = ( DerivedFigure )myProvider.ProvideValue( myContext.Object );

            Assert.That( result.Currency, Is.EqualTo( myRhsSeries.Currency ) );
            Assert.That( result.Value, Is.EqualTo( 17.21 * 2 + 23 ) );
        }

        /// <summary>
        /// Consier Market capitalization which is price * shares outstanding ( no currency for shares outstanding)
        /// </summary>
        [Test]
        public void ProvideValue_SeriesWithoutCurrency_CurrencyOfPriceTaken()
        {
            myCurrentPrice = FigureFactory.NewPrice( "2015-01-01", 17.21, Euro );
            myRhsSeries = new FigureSeries( typeof( FakeFigure ), FigureFactory.New( 2015, 23 ), FigureFactory.New( 2014, 37 ) );

            var result = ( DerivedFigure )myProvider.ProvideValue( myContext.Object );

            Assert.That( result.Currency, Is.EqualTo( myCurrentPrice.Currency ) );
            Assert.That( result.Value, Is.EqualTo( 17.21 + 23 ) );
        }
    }
}
