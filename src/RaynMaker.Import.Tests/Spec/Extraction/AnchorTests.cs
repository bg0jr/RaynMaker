using System.Runtime.Serialization;
using NUnit.Framework;
using RaynMaker.Import.Spec.v2.Extraction;
using RaynMaker.Import.Tests;

namespace RaynMaker.Import.Tests.Spec.Extraction
{
    [TestFixture]
    public class AnchroTests
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var anchor = TableFragmentAnchor.ForCell( new AbsolutePositionLocator( 4 ), new AbsolutePositionLocator( 8 ) );

            var clone = FormatFactory.Clone( anchor );

            Assert.That( ( ( AbsolutePositionLocator )clone.Row ).Position, Is.EqualTo( 4 ) );
            Assert.That( ( ( AbsolutePositionLocator )clone.Column ).Position, Is.EqualTo( 8 ) );
        }
    }
}
