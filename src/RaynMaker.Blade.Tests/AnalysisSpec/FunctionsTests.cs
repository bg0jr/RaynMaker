using System.Linq;
using NUnit.Framework;
using RaynMaker.Blade.AnalysisSpec;
using RaynMaker.Blade.DataSheetSpec;

namespace RaynMaker.Blade.Tests.AnalysisSpec
{
    [TestFixture]
    public class FunctionsTests
    {
        [Test]
        public void Last_WithZero_ReturnsEmptyCollection()
        {
            var series = new[] { 1, 2, 3, 4, 5 }
                .Select( v => new Datum { Value = v } );

            var result = Functions.Last( series, 0 );

            Assert.That( result, Is.Empty );
        }

        [Test]
        public void Last_WithOne_ReturnsCollectionWithLastItem()
        {
            var series = new[] { 1, 2, 3, 4, 5 }
                .Select( v => new Datum { Value = v } )
                .ToList();

            var result = Functions.Last( series, 1 );

            var expected = new[] { series.Last() };

            Assert.That( result, Is.EquivalentTo( expected ) );
        }

        [Test]
        public void Last_WithThree_ReturnsCollectionWithLastThreeItems()
        {
            var series = new[] { 1, 2, 3, 4, 5 }
                .Select( v => new Datum { Value = v } )
                .ToList();

            var result = Functions.Last( series, 3 );

            var expected = new[] { series[ 2 ], series[ 3 ], series[ 4 ] };

            Assert.That( result, Is.EquivalentTo( expected ) );
        }
    }
}
