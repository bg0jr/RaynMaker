using System;
using NUnit.Framework;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.Tests.Spec.Extraction
{
    [TestFixture]
    public class StringContainsLocatorTests
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
