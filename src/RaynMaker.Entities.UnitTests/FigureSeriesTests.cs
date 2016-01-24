using System;
using System.Linq;
using NUnit.Framework;
using RaynMaker.Entities.Figures;
using RaynMaker.Entities.UnitTests.Fakes;

namespace RaynMaker.Entities.UnitTests.Entities
{
    [TestFixture]
    public class FigureSeriesTests
    {
        private static readonly Currency Euro = new Currency { Name = "Euro" };
        private static readonly Currency Dollar = new Currency { Name = "Dollar" };

        [Test]
        public void Ctor_WhenCalled_DataTypeSet()
        {
            var series = new FigureSeries( typeof( Equity ) );

            Assert.That( series.FigureType, Is.EqualTo( typeof( Equity ) ) );
        }

        [Test]
        public void Ctor_WhenCalled_NameIsSet()
        {
            var series = new FigureSeries( typeof( Equity ) );

            Assert.That( series.Name, Is.StringContaining( "Equity" ) );
        }

        [Test]
        public void Ctor_WithElements_ElementsAddedInOrder()
        {
            var series = new FigureSeries( typeof( FakeCurrencyFigure ), FigureFactory.New( 2015, 5, Euro ), FigureFactory.New( 2014, 7, Euro ) );

            Assert.That( series.First().Period, Is.EqualTo( new YearPeriod( 2014 ) ) );
            Assert.That( series.Last().Period, Is.EqualTo( new YearPeriod( 2015 ) ) );
        }

        [Test]
        public void Ctor_WithElements_CurrencyIsTakenOver()
        {
            var series = new FigureSeries( typeof( FakeCurrencyFigure ), FigureFactory.New( 2015, 5, Euro ), FigureFactory.New( 2014, 7, Euro ) );

            Assert.That( series.Currency, Is.EqualTo( Euro ) );
        }

        [Test]
        public void Add_WhenCalled_ElementsAddedInOrder()
        {
            var series = new FigureSeries( typeof( FakeCurrencyFigure ) );

            series.Add( FigureFactory.New( 2015, 5, Euro ) );
            series.Add( FigureFactory.New( 2014, 7, Euro ) );

            Assert.That( series.First().Period, Is.EqualTo( new YearPeriod( 2014 ) ) );
            Assert.That( series.Last().Period, Is.EqualTo( new YearPeriod( 2015 ) ) );
        }

        [Test]
        public void Add_WithICurrencyFigure_CurrencyIsTakenOver()
        {
            var series = new FigureSeries( typeof( FakeCurrencyFigure ) );

            series.Add( FigureFactory.New( 2015, 5, Euro ) );
            series.Add( FigureFactory.New( 2014, 7, Euro ) );

            Assert.That( series.Currency, Is.EqualTo( Euro ) );
        }

        [Test]
        public void Add_InvalidCurrency_Throws()
        {
            var series = new FigureSeries( typeof( FakeCurrencyFigure ) );

            series.Add( FigureFactory.New( 2015, 5, Euro ) );
            var ex = Assert.Throws<ArgumentException>( () => series.Add( FigureFactory.New( 2014, 7, Dollar ) ) );
            Assert.That( ex.Message, Is.StringContaining( "Currency inconsistencies" ) );
        }

        [Test]
        public void Add_MissingCurrencyWhenCurrencyExpected_Throws()
        {
            var series = new FigureSeries( typeof( FakeCurrencyFigure ) );

            series.Add( FigureFactory.New( 2015, 5, Euro ) );
            var ex = Assert.Throws<ArgumentException>( () => series.Add( FigureFactory.New( 2014, 7, null ) ) );
            Assert.That( ex.Message, Is.StringContaining( "Currency inconsistencies" ) );
        }

        [Test]
        public void Add_CurrencyUnexpected_Throws()
        {
            var series = new FigureSeries( typeof( FakeCurrencyFigure ) );

            series.Add( FigureFactory.New( 2015, 5, null ) );
            var ex = Assert.Throws<ArgumentException>( () => series.Add( FigureFactory.New( 2014, 7, Dollar ) ) );
            Assert.That( ex.Message, Is.StringContaining( "Currency inconsistencies" ) );
        }

        [Test]
        public void Add_FigureTypeMismatch_Throws()
        {
            var series = new FigureSeries( typeof( FakeFigure ) );

            series.Add( FigureFactory.New( 2015, 5 ) );
            var ex = Assert.Throws<ArgumentException>( () => series.Add( FigureFactory.NewPrice( "2013-11-11", 7, Dollar ) ) );
            Assert.That( ex.Message, Is.StringContaining( "FigureType mismatch" ) );
        }

    }
}
