using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.SDK;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Extraction
{
    [TestFixture]
    public class StringContainsLocatorTests
    {
        [Test]
        public void HeaderSeriesPosition_Set_ValueIsSet()
        {
            var locator = new StringContainsLocator();

            locator.HeaderSeriesPosition = 6;

            Assert.That( locator.HeaderSeriesPosition, Is.EqualTo( 6 ) );
        }

        [Test]
        public void HeaderSeriesPosition_Set_ChangeIsNotified()
        {
            var locator = new StringContainsLocator();
            var counter = new PropertyChangedCounter( locator );

            locator.HeaderSeriesPosition = 6;

            Assert.That( counter.GetCount( () => locator.HeaderSeriesPosition ), Is.EqualTo( 1 ) );
        }

        [Test]
        public void Pattern_Set_ValueIsSet()
        {
            var locator = new StringContainsLocator();

            locator.Pattern = "EPS";

            Assert.That( locator.Pattern.ToString(), Is.EqualTo( "EPS" ) );
        }

        [Test]
        public void Pattern_Set_ChangeIsNotified()
        {
            var locator = new StringContainsLocator();
            var counter = new PropertyChangedCounter( locator );

            locator.Pattern = "EPS";

            Assert.That( counter.GetCount( () => locator.Pattern ), Is.EqualTo( 1 ) );
        }
        
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
        public void Validate_HeaderSeriesPositionOutOfRange_Throws()
        {
            var locator = new StringContainsLocator { HeaderSeriesPosition = -1, Pattern = "Sales" };

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( locator ) );
            Assert.That( ex.Message, Is.StringContaining( "HeaderSeriesPosition must be between 0 and " + int.MaxValue ) );
        }

        [Test]
        public void Validate_PatternNull_Throws()
        {
            var locator = new StringContainsLocator { HeaderSeriesPosition = 0, Pattern = null };

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( locator ) );
            Assert.That( ex.Message, Is.StringContaining( "Pattern field is required" ) );
        }
    }
}
