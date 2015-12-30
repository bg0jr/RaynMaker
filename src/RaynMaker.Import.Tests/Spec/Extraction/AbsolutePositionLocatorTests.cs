using NUnit.Framework;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2.Extraction;

namespace RaynMaker.Import.Tests.Spec.Extraction
{
    [TestFixture]
    class AbsolutePositionLocatorTests
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var locator = new AbsolutePositionLocator( 17 );

            var clone = FormatFactory.Clone( locator );

            Assert.That( clone.Position, Is.EqualTo( locator.Position ) );
            Assert.That( clone.SeriesToScan, Is.EqualTo( locator.SeriesToScan ) );
        }
    }
}
