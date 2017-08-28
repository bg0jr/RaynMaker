using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.SDK;

namespace RaynMaker.Modules.Import.Tests.Spec.Extraction
{
    [TestFixture]
    public class RegexPatternLocatorTests
    {
        [Test]
        public void HeaderSeriesPosition_Set_ValueIsSet()
        {
            var locator = new RegexPatternLocator();

            locator.HeaderSeriesPosition = 6;

            Assert.That(locator.HeaderSeriesPosition, Is.EqualTo(6));
        }

        [Test]
        public void HeaderSeriesPosition_Set_ChangeIsNotified()
        {
            var locator = new RegexPatternLocator();
            var counter = new PropertyChangedCounter(locator);

            locator.HeaderSeriesPosition = 6;

            Assert.That(counter.GetCount(nameof(locator.HeaderSeriesPosition)), Is.EqualTo(1));
        }

        [Test]
        public void Pattern_Set_ValueIsSet()
        {
            var locator = new RegexPatternLocator();

            locator.Pattern = new Regex("Current*");

            Assert.That(locator.Pattern.ToString(), Is.EqualTo("Current*"));
        }

        [Test]
        public void Pattern_Set_ChangeIsNotified()
        {
            var locator = new RegexPatternLocator();
            var counter = new PropertyChangedCounter(locator);

            locator.Pattern = new Regex("Current*");

            Assert.That(counter.GetCount(nameof(locator.Pattern)), Is.EqualTo(1));
        }

        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var locator = new RegexPatternLocator { HeaderSeriesPosition = 4, Pattern = new Regex("^.*$") };

            var clone = FigureDescriptorFactory.Clone(locator);

            Assert.That(clone.HeaderSeriesPosition, Is.EqualTo(4));
            Assert.That(clone.Pattern.ToString(), Is.EqualTo("^.*$"));
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var locator = new RegexPatternLocator { HeaderSeriesPosition = 4, Pattern = new Regex("^.*$") };

            RecursiveValidator.Validate(locator);
        }

        [Test]
        public void Validate_HeaderSeriesPositionOutOfRange_Throws()
        {
            var locator = new RegexPatternLocator { HeaderSeriesPosition = -1, Pattern = new Regex("^.*$") };

            var ex = Assert.Throws<ValidationException>(() => RecursiveValidator.Validate(locator));
            Assert.That(ex.Message, Does.Contain("HeaderSeriesPosition must be between 0 and " + int.MaxValue));
        }

        [Test]
        public void Validate_PatternNull_Throws()
        {
            var locator = new RegexPatternLocator { HeaderSeriesPosition = 0, Pattern = null };

            var ex = Assert.Throws<ValidationException>(() => RecursiveValidator.Validate(locator));
            Assert.That(ex.Message, Does.Contain("Pattern field is required"));
        }
    }
}
