using System;
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
            var locator = new AbsolutePositionLocator { HeaderSeriesPosition = 6, SeriesPosition = 17 };

            var clone = FigureDescriptorFactory.Clone( locator );

            Assert.That( clone.SeriesPosition, Is.EqualTo( locator.SeriesPosition ) );
            Assert.That( clone.HeaderSeriesPosition, Is.EqualTo( locator.HeaderSeriesPosition ) );
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var locator = new AbsolutePositionLocator { HeaderSeriesPosition = 6, SeriesPosition = 17 };

            RecursiveValidator.Validate( locator );
        }

        [Test]
        public void Ctor_HeaderSeriesPositionOutOfRange_Throws()
        {
            var ex = Assert.Throws<ArgumentException>( () => new AbsolutePositionLocator(/* -1, 17 */) );
            Assert.That( ex.Message, Is.StringContaining( "HeaderSeriesPosition must be greater or equal to 0" ) );
        }

        [Test]
        public void Ctor_SeriesPositionOutOfRange_Throws()
        {
            var ex = Assert.Throws<ArgumentException>( () => new AbsolutePositionLocator( /*0, -1 */) );
            Assert.That( ex.Message, Is.StringContaining( "SeriesPosition must be greater or equal to 0" ) );
        }
    }
}
