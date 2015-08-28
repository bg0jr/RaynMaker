using System.Linq;
using NUnit.Framework;
using RaynMaker.Analysis.AnalysisSpec;
using RaynMaker.Entities;
using RaynMaker.Entities.Datums;
using RaynMaker.Entities.Tests.Fakes;

namespace RaynMaker.Analysis.Tests.AnalysisSpec
{
    [TestFixture]
    public class FunctionsTests
    {
        [Test]
        public void LastN_WithZero_ReturnsEmptyCollection()
        {
            var series = new[] { 1, 2, 3, 4, 5 }
                .Select( v => new FakeDatum { Value = v } );

            var result = Functions.LastN( series, 0 );

            Assert.That( result, Is.Empty );
        }

        [Test]
        public void LastN_WithOne_ReturnsCollectionWithLastItem()
        {
            var series = new[] { 1, 2, 3, 4, 5 }
                .Select( v => new FakeDatum { Value = v } )
                .ToList();

            var result = Functions.LastN( series, 1 );

            var expected = new[] { series.Last() };

            Assert.That( result, Is.EquivalentTo( expected ) );
        }

        [Test]
        public void LastN_WithThree_ReturnsCollectionWithLastThreeItems()
        {
            var series = new[] { 1, 2, 3, 4, 5 }
                .Select( v => new FakeDatum { Value = v } )
                .ToList();

            var result = Functions.LastN( series, 3 );

            var expected = new[] { series[ 2 ], series[ 3 ], series[ 4 ] };

            Assert.That( result, Is.EquivalentTo( expected ) );
        }

        [Test]
        public void Average_WhenCalled_ReturnsAverage()
        {
            var series = new[] { 1d, 2d, 3d, 4d, 5d }
                .Select( v => new DerivedDatum { Value = v } )
                .ToList();

            var result = Functions.Average( series );

            Assert.That( result.Value, Is.EqualTo( 3 ) );
        }

        [Test]
        public void Average_InputHasCurrency_ReturnsAverageWithCurrency()
        {
            var currency = new Currency { Name = "Euro" };

            var series = new[] { 1d, 2d, 3d, 4d, 5d }
                .Select( v => new Equity
                {
                    Value = v,
                    Currency = currency
                } )
                .ToList();

            var result = Functions.Average( series );

            Assert.That( ( ( ICurrencyDatum )result ).Currency, Is.EqualTo( currency ) );
        }
    }
}
