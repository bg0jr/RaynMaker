using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.SDK;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Extraction
{
    [TestFixture]
    class AbsolutePositionLocatorTests
    {
        [Test]
        public void HeaderSeriesPosition_Set_ValueIsSet()
        {
            var locator = new AbsolutePositionLocator();

            locator.HeaderSeriesPosition = 6;

            Assert.That( locator.HeaderSeriesPosition, Is.EqualTo( 6 ) );
        }

        [Test]
        public void HeaderSeriesPosition_Set_ChangeIsNotified()
        {
            var locator = new AbsolutePositionLocator();
            var counter = new PropertyChangedCounter( locator );

            locator.HeaderSeriesPosition = 6;

            Assert.That( counter.GetCount(nameof(locator.HeaderSeriesPosition) ), Is.EqualTo( 1 ) );
        }

        [Test]
        public void SeriesPosition_Set_ValueIsSet()
        {
            var locator = new AbsolutePositionLocator();

            locator.SeriesPosition = 6;

            Assert.That( locator.SeriesPosition, Is.EqualTo( 6 ) );
        }

        [Test]
        public void SeriesPosition_Set_ChangeIsNotified()
        {
            var locator = new AbsolutePositionLocator();
            var counter = new PropertyChangedCounter( locator );

            locator.SeriesPosition = 6;

            Assert.That( counter.GetCount(nameof(locator.SeriesPosition )), Is.EqualTo( 1 ) );
        }

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
        public void Validate_HeaderSeriesPositionOutOfRange_Throws()
        {
            var locator = new AbsolutePositionLocator { HeaderSeriesPosition = -1, SeriesPosition = 17 };

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( locator ) );
            Assert.That( ex.Message, Does.Contain( "HeaderSeriesPosition must be between 0 and " + int.MaxValue ) );
        }

        [Test]
        public void Validate_SeriesPositionOutOfRange_Throws()
        {
            var locator = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = -1 };

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( locator ) );
            Assert.That( ex.Message, Does.Contain( "SeriesPosition must be between 0 and " + int.MaxValue ) );
        }
    }
}
