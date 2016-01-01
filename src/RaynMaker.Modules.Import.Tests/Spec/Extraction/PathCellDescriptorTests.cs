using System;
using NUnit.Framework;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Extraction
{
    [TestFixture]
    public class PathCellDescriptorTests
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var format = new PathCellDescriptor( "dummy" );
            format.Path = "123";
            format.Column = new AbsolutePositionLocator(0, 4 );
            format.Row = new AbsolutePositionLocator( 0, 23 );
            format.ValueFormat = new ValueFormat( typeof( double ), "0.00" );
            format.Currency = "Euro";

            var clone = FormatFactory.Clone( format );

            Assert.That( clone.Path, Is.EqualTo( "123" ) );

            Assert.That( ( ( AbsolutePositionLocator )clone.Column ).SeriesPosition, Is.EqualTo( 4 ) );
            Assert.That( ( ( AbsolutePositionLocator )clone.Row ).SeriesPosition, Is.EqualTo( 23 ) );

            Assert.That( clone.ValueFormat.Type, Is.EqualTo( format.ValueFormat.Type ) );
            Assert.That( clone.ValueFormat.Format, Is.EqualTo( format.ValueFormat.Format ) );

            Assert.That( clone.Currency, Is.EqualTo( "Euro" ) );
        }
    }
}
