using System;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Extraction
{
    [TestFixture]
    public class StringContainsLocatorTests
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var locator = new StringContainsLocator { HeaderSeriesPosition = 4, Pattern = "Sales" };

            var clone = FigureDescriptorFactory.Clone( locator );

            Assert.That( clone.HeaderSeriesPosition, Is.EqualTo( 4 ) );
            Assert.That( clone.Pattern, Is.EqualTo( "Sales" ) );
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var locator = new StringContainsLocator { HeaderSeriesPosition = 4, Pattern = "Sales" };

            RecursiveValidator.Validate( locator );
        }

        [Test]
        public void Ctor_HeaderSeriesPositionOutOfRange_Throws()
        {
            var ex = Assert.Throws<ArgumentException>( () => new StringContainsLocator( /*-1, "Sales"*/ ) );
            Assert.That( ex.Message, Is.StringContaining( "HeaderSeriesPosition must be greater or equal to 0" ) );
        }

        [Test]
        public void Ctor_PatternInvalid_Throws( [Values( null, "" )] string pattern )
        {
            var ex = Assert.Throws<ArgumentNullException>( () => new StringContainsLocator(/* 4, pattern */) );
            Assert.That( ex.Message, Is.StringContaining( "string must not null or empty: pattern" ) );
        }
    }
}
