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
        public void Clone_ForCell_AllMembersAreCloned()
        {
            var anchor = TableFragmentDescriptor.ForCell( new AbsolutePositionLocator( 4 ), new AbsolutePositionLocator( 8 ) );

            var clone = FormatFactory.Clone( anchor );

            Assert.That( clone.Expand, Is.EqualTo( CellDimension.None ) );
            Assert.That( ( ( AbsolutePositionLocator )clone.Row ).Position, Is.EqualTo( 4 ) );
            Assert.That( ( ( AbsolutePositionLocator )clone.Column ).Position, Is.EqualTo( 8 ) );
        }

        [Test]
        public void Clone_ForRow_AllMembersAreCloned()
        {
            var anchor = TableFragmentDescriptor.ForRow( new AbsolutePositionLocator( 8 ) );

            var clone = FormatFactory.Clone( anchor );

            Assert.That( clone.Expand, Is.EqualTo( CellDimension.Row ) );
            Assert.That( ( ( AbsolutePositionLocator )clone.Row ).Position, Is.EqualTo( 4 ) );
            Assert.That( clone.Column, Is.Null );
        }

        [Test]
        public void Clone_ForColumn_AllMembersAreCloned()
        {
            var anchor = TableFragmentDescriptor.ForColumn( new AbsolutePositionLocator( 8 ) );

            var clone = FormatFactory.Clone( anchor );

            Assert.That( clone.Expand, Is.EqualTo( CellDimension.Column ) );
            Assert.That( clone.Row, Is.Null );
            Assert.That( ( ( AbsolutePositionLocator )clone.Column ).Position, Is.EqualTo( 8 ) );
        }
    }
}
