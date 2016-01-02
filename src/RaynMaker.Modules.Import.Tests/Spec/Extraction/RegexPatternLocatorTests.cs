using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Extraction
{
    [TestFixture]
    public class RegexPatternLocatorTests
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var locator = new RegexPatternLocator { HeaderSeriesPosition = 4, Pattern = new Regex( "^.*$" ) };

            var clone = FigureDescriptorFactory.Clone( locator );

            Assert.That( clone.HeaderSeriesPosition, Is.EqualTo( 4 ) );
            Assert.That( clone.Pattern.ToString(), Is.EqualTo( "^.*$" ) );
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var locator = new RegexPatternLocator { HeaderSeriesPosition = 4, Pattern = new Regex( "^.*$" ) };

            RecursiveValidator.Validate( locator );
        }

        [Test]
        public void Ctor_HeaderSeriesPositionOutOfRange_Throws()
        {
            var ex = Assert.Throws<ArgumentException>( () => new RegexPatternLocator(/* -1, "^.*$"*/ ) );
            Assert.That( ex.Message, Is.StringContaining( "HeaderSeriesPosition must be greater or equal to 0" ) );
        }

        [Test]
        public void Ctor_PatternNull_Throws()
        {
            var ex = Assert.Throws<ArgumentNullException>( () => new RegexPatternLocator(/* 0, ( Regex )null*/ ) );
            Assert.That( ex.Message, Is.StringContaining( "Value cannot be null." + Environment.NewLine + "Parameter name: pattern" ) );
        }

        [Test]
        public void Ctor_PatternEmpty_Throws()
        {
            var ex = Assert.Throws<ArgumentNullException>( () => new RegexPatternLocator(/* 0, string.Empty*/ ) );
            Assert.That( ex.Message, Is.StringContaining( "string must not null or empty: pattern" ) );
        }
    }
}
