using System;
using NUnit.Framework;
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
            var format = new RegexPatternLocator( 4, "^.*$" );

            var clone = FormatFactory.Clone( format );

            Assert.That( clone.HeaderSeriesPosition, Is.EqualTo( 4 ) );
            Assert.That( clone.Pattern.ToString(), Is.EqualTo( "^.*$" ) );
        }
    }
}
