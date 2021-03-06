﻿using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using RaynMaker.Modules.Analysis.AnalysisSpec.Providers;
using RaynMaker.Modules.Analysis.Engine;
using RaynMaker.Entities;
using RaynMaker.Entities.Tests.Fakes;

namespace RaynMaker.Modules.Analysis.Tests.AnalysisSpec.Providers
{
    [TestFixture]
    public class GenericJoinProviderTests
    {
        private const string LhsSeriesName = "S1";
        private const string RhsSeriesName = "S2";

        private static readonly Currency Euro = new Currency { Name = "Euro" };
        private static readonly Currency Dollar = new Currency { Name = "Dollar" };

        private Mock<IFigureProviderContext> myContext;
        private GenericJoinProvider myProvider;
        private IFigureSeries myLhsSeries;
        private IFigureSeries myRhsSeries;

        [SetUp]
        public void SetUp()
        {
            myContext = new Mock<IFigureProviderContext> { DefaultValue = DefaultValue.Mock };
            myContext.Setup( x => x.GetSeries( LhsSeriesName ) ).Returns( () => myLhsSeries );
            myContext.Setup( x => x.GetSeries( RhsSeriesName ) ).Returns( () => myRhsSeries );

            myProvider = new GenericJoinProvider( "dummy", LhsSeriesName, RhsSeriesName, ( lhs, rhs ) => lhs + rhs );
        }

        [TearDown]
        public void TearDown()
        {
            myContext = null;
            myProvider = null;
            myLhsSeries = null;
            myRhsSeries = null;
        }

        [Test]
        public void ProvideValue_LhsSeriesEmpty_ReturnsMissingData()
        {
            myLhsSeries = FigureSeries.Empty;
            myRhsSeries = new FigureSeries( typeof( FakeFigure ), FigureFactory.New( 2015, 1 ) );

            var result = myProvider.ProvideValue( myContext.Object );

            Assert.That( result, Is.InstanceOf<MissingData>() );
            Assert.That( ( ( MissingData )result ).Figure, Is.EqualTo( LhsSeriesName ) );
        }

        [Test]
        public void ProvideValue_RhsSeriesEmpty_ReturnsMissingData()
        {
            myLhsSeries = new FigureSeries( typeof( FakeFigure ), FigureFactory.New( 2015, 1 ) );
            myRhsSeries = FigureSeries.Empty;

            var result = myProvider.ProvideValue( myContext.Object );

            Assert.That( result, Is.InstanceOf<MissingData>() );
            Assert.That( ( ( MissingData )result ).Figure, Is.EqualTo( RhsSeriesName ) );
        }

        [Test]
        public void ProvideValue_RhsHasNoDataForPeriod_ItemSkippedInJoin()
        {
            myLhsSeries = new FigureSeries( typeof( FakeFigure ), FigureFactory.New( 2015, 5 ), FigureFactory.New( 2014, 7 ), FigureFactory.New( 2013, 87 ) );
            myRhsSeries = new FigureSeries( typeof( FakeFigure ), FigureFactory.New( 2015, 23 ), FigureFactory.New( 2014, 37 ) );

            var result = ( IFigureSeries )myProvider.ProvideValue( myContext.Object );

            Assert.That( result.Count, Is.EqualTo( 2 ) );
        }

        [Test]
        public void ProvideValue_WithValidInputData_JoinReturned()
        {
            myLhsSeries = new FigureSeries( typeof( FakeFigure ), FigureFactory.New( 2015, 5 ), FigureFactory.New( 2014, 7 ) );
            myRhsSeries = new FigureSeries( typeof( FakeFigure ), FigureFactory.New( 2015, 23 ), FigureFactory.New( 2014, 37 ) );

            var result = ( IFigureSeries )myProvider.ProvideValue( myContext.Object );

            var r2015 = result.Single( i => i.Period.Equals( new YearPeriod( 2015 ) ) );
            Assert.That( r2015.Value, Is.EqualTo( 28 ) );

            var r2014 = result.Single( i => i.Period.Equals( new YearPeriod( 2014 ) ) );
            Assert.That( r2014.Value, Is.EqualTo( 44 ) );
        }

        [Test]
        public void ProvideValue_WithPreserveCurrency_CurrencyTakenOverForResult()
        {
            myLhsSeries = new FigureSeries( typeof( FakeCurrencyFigure ), FigureFactory.New( 2015, 5, Euro ), FigureFactory.New( 2014, 7, Euro ) );
            myRhsSeries = new FigureSeries( typeof( FakeCurrencyFigure ), FigureFactory.New( 2015, 23, Euro ), FigureFactory.New( 2014, 37, Euro ) );

            var result = ( IFigureSeries )myProvider.ProvideValue( myContext.Object );

            Assert.That( result.Currency, Is.EqualTo( Euro ) );
        }

        [Test]
        public void ProvideValue_WhenCalled_InputsReferenced()
        {
            myLhsSeries = new FigureSeries( typeof( FakeCurrencyFigure ), FigureFactory.New( 2015, 5, Euro ), FigureFactory.New( 2014, 7, Euro ) );
            myRhsSeries = new FigureSeries( typeof( FakeCurrencyFigure ), FigureFactory.New( 2015, 23, Euro ), FigureFactory.New( 2014, 37, Euro ) );

            var result = ( IFigureSeries )myProvider.ProvideValue( myContext.Object );

            Assert.That( ( ( DerivedFigure )result.ElementAt( 0 ) ).Inputs, Is.EquivalentTo( new[] { myLhsSeries.ElementAt( 0 ), myRhsSeries.ElementAt( 0 ) } ) );
            Assert.That( ( ( DerivedFigure )result.ElementAt( 1 ) ).Inputs, Is.EquivalentTo( new[] { myLhsSeries.ElementAt( 1 ), myRhsSeries.ElementAt( 1 ) } ) );
        }

        [Test]
        public void ProvideValue_InconsistentCurrencies_Throws()
        {
            myLhsSeries = new FigureSeries( typeof( FakeCurrencyFigure ), FigureFactory.New( 2015, 5, Euro ), FigureFactory.New( 2014, 7, Euro ) );
            myRhsSeries = new FigureSeries( typeof( FakeCurrencyFigure ), FigureFactory.New( 2015, 23, Dollar ), FigureFactory.New( 2014, 37, Dollar ) );

            var ex = Assert.Throws<ArgumentException>( () => myProvider.ProvideValue( myContext.Object ) );
            Assert.That( ex.Message, Does.Contain( "Currency inconsistencies" ) );
        }
    }
}
