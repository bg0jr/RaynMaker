using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Extraction
{
    [TestFixture]
    class AbsolutePositionLocatorTests
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var locator = new AbsolutePositionLocator( 6, 17 );

            var clone = FormatFactory.Clone( locator );

            Assert.That( clone.SeriesPosition, Is.EqualTo( locator.SeriesPosition ) );
            Assert.That( clone.HeaderSeriesPosition, Is.EqualTo( locator.HeaderSeriesPosition ) );
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var locator = new AbsolutePositionLocator( 1, 17 );

            RecursiveValidator.Validate( locator );
        }

        [Test]
        public void Validate_HeaderSeriesPositionOutOfRange_Throws()
        {
            var locator = new AbsolutePositionLocator( -1, 17 );

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( locator ) );
            Assert.That( ex.Message, Is.StringContaining( "HeaderSeriesPosition must be between 0 and" ) );
        }

        [Test]
        public void Validate_SeriesPositionOutOfRange_Throws()
        {
            var locator = new AbsolutePositionLocator( 0, -1 );

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( locator ) );
            Assert.That( ex.Message, Is.StringContaining( "SeriesPosition must be between 0 and" ) );
        }
    }
}
