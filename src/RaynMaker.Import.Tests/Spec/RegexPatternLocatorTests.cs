using System;
using NUnit.Framework;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Tests.Spec
{
    [TestFixture]
    public class RegexPatternLocatorTests : TestBase
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var format = new RegexPatternLocator( 4, "^.*$" );

            var clone = FormatFactory.Clone( format );

            Assert.That( clone.SeriesToScan, Is.EqualTo( 4 ) );
            Assert.That( clone.Pattern.ToString(), Is.EqualTo( "^.*$" ) );
        }
    }
}
