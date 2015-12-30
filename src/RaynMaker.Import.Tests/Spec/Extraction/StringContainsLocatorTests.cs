using System;
using NUnit.Framework;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2.Extraction;

namespace RaynMaker.Import.Tests.Spec.Extraction
{
    [TestFixture]
    public class StringContainsLocatorTests : TestBase
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var format = new StringContainsLocator( 4, "Sales" );

            var clone = FormatFactory.Clone( format );

            Assert.That( clone.SeriesToScan, Is.EqualTo( 4 ) );
            Assert.That( clone.Pattern, Is.EqualTo( "Sales" ) );
        }
    }
}
