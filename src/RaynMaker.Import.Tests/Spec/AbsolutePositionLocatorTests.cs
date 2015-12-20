using NUnit.Framework;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Tests.Spec
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
