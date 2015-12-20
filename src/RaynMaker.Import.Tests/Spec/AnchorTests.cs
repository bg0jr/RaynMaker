using System.Runtime.Serialization;
using NUnit.Framework;
using RaynMaker.Import.Tests;

namespace RaynMaker.Import.Spec
{
    [TestFixture]
    public class AnchroTests : TestBase
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var anchor = Anchor.ForCell( new AbsolutePositionLocator( 4 ), new AbsolutePositionLocator( 8 ) );

            var clone = FormatFactory.Clone( anchor );

            Assert.That( ( ( AbsolutePositionLocator )clone.Row ).Position, Is.EqualTo( 4 ) );
            Assert.That( ( ( AbsolutePositionLocator )clone.Column ).Position, Is.EqualTo( 8 ) );
        }
    }
}
