using System;
using NUnit.Framework;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2.Extraction;

namespace RaynMaker.Import.Tests.Spec.Extraction
{
    [TestFixture]
    public class RegexPatternLocatorTests
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
