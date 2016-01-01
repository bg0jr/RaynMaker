using NUnit.Framework;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Extraction
{
    [TestFixture]
    class AbsolutePositionLocatorTests
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var locator = new AbsolutePositionLocator( 6, 17 );

            var clone = FormatFactory.Clone( locator );

            Assert.That( clone.Position, Is.EqualTo( locator.Position ) );
            Assert.That( clone.HeaderSeriesPosition, Is.EqualTo( locator.HeaderSeriesPosition ) );
        }
    }
}
