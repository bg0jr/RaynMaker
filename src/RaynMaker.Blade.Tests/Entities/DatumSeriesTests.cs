using NUnit.Framework;
using System.Linq;
using RaynMaker.Blade.Entities;
using RaynMaker.Blade.Entities.Datums;
using RaynMaker.Blade.Tests.Fakes;
using System;
using RaynMaker.Entities;

namespace RaynMaker.Blade.Tests.Entities
{
    [TestFixture]
    public class DatumSeriesTests
    {
        private static readonly Currency Euro = new Currency { Name = "Euro" };
        private static readonly Currency Dollar = new Currency { Name = "Dollar" };

        [Test]
        public void Ctor_WhenCalled_DataTypeSet()
        {
            var series = new DatumSeries( typeof( Equity ) );

            Assert.That( series.DatumType, Is.EqualTo( typeof( Equity ) ) );
        }

        [Test]
        public void Ctor_WhenCalled_NameIsSet()
        {
            var series = new DatumSeries( typeof( Equity ) );

            Assert.That( series.Name, Is.StringContaining( "Equity" ) );
        }

        [Test]
        public void Ctor_WithElements_ElementsAddedInOrder()
        {
            var series = new DatumSeries( typeof( CurrencyDatum ), DatumFactory.New( 2015, 5, Euro ), DatumFactory.New( 2014, 7, Euro ) );

            Assert.That( series.First().Period, Is.EqualTo( new YearPeriod( 2014 ) ) );
            Assert.That( series.Last().Period, Is.EqualTo( new YearPeriod( 2015 ) ) );
        }

        [Test]
        public void Ctor_WithElements_CurrencyIsTakenOver()
        {
            var series = new DatumSeries( typeof( CurrencyDatum ), DatumFactory.New( 2015, 5, Euro ), DatumFactory.New( 2014, 7, Euro ) );

            Assert.That( series.Currency, Is.EqualTo( Euro ) );
        }

        [Test]
        public void Add_WhenCalled_ElementsAddedInOrder()
        {
            var series = new DatumSeries( typeof( CurrencyDatum ) );

            series.Add( DatumFactory.New( 2015, 5, Euro ) );
            series.Add( DatumFactory.New( 2014, 7, Euro ) );

            Assert.That( series.First().Period, Is.EqualTo( new YearPeriod( 2014 ) ) );
            Assert.That( series.Last().Period, Is.EqualTo( new YearPeriod( 2015 ) ) );
        }

        [Test]
        public void Add_WithICurrencyDatum_CurrencyIsTakenOver()
        {
            var series = new DatumSeries( typeof( CurrencyDatum ) );

            series.Add( DatumFactory.New( 2015, 5, Euro ) );
            series.Add( DatumFactory.New( 2014, 7, Euro ) );

            Assert.That( series.Currency, Is.EqualTo( Euro ) );
        }

        [Test]
        public void Add_InvalidCurrency_Throws()
        {
            var series = new DatumSeries( typeof( CurrencyDatum ) );

            series.Add( DatumFactory.New( 2015, 5, Euro ) );
            var ex = Assert.Throws<ArgumentException>( () => series.Add( DatumFactory.New( 2014, 7, Dollar ) ) );
            Assert.That( ex.Message, Is.StringContaining( "Currency inconsistencies" ) );
        }

        [Test]
        public void Add_MissingCurrencyWhenCurrencyExpected_Throws()
        {
            var series = new DatumSeries( typeof( CurrencyDatum ) );

            series.Add( DatumFactory.New( 2015, 5, Euro ) );
            var ex = Assert.Throws<ArgumentException>( () => series.Add( DatumFactory.New( 2014, 7, null ) ) );
            Assert.That( ex.Message, Is.StringContaining( "Currency inconsistencies" ) );
        }

        [Test]
        public void Add_CurrencyUnexpected_Throws()
        {
            var series = new DatumSeries( typeof( CurrencyDatum ) );

            series.Add( DatumFactory.New( 2015, 5, null ) );
            var ex = Assert.Throws<ArgumentException>( () => series.Add( DatumFactory.New( 2014, 7, Dollar ) ) );
            Assert.That( ex.Message, Is.StringContaining( "Currency inconsistencies" ) );
        }

        [Test]
        public void Add_DatumTypeMismatch_Throws()
        {
            var series = new DatumSeries( typeof( Datum ) );

            series.Add( DatumFactory.New( 2015, 5 ) );
            var ex = Assert.Throws<ArgumentException>( () => series.Add( DatumFactory.NewPrice( "2013-11-11", 7, Dollar ) ) );
            Assert.That( ex.Message, Is.StringContaining( "DatumType mismatch" ) );
        }

    }
}
