using System;
using NUnit.Framework;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2.Extraction;

namespace RaynMaker.Import.Tests.Spec.Extraction
{
    [TestFixture]
    public class PathCellFormatTests
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var format = new PathCellDescriptor( "dummy" );
            format.Path = "123";
            format.Column = new AbsolutePositionLocator( 4 );
            format.Row = new AbsolutePositionLocator( 23 );
            format.ValueFormat = new ValueFormat( typeof( double ), "0.00" );
            format.Currency = "Euro";

            var clone = FormatFactory.Clone( format );

            Assert.That( clone.Path, Is.EqualTo( "123" ) );

            Assert.That( ( ( AbsolutePositionLocator )clone.Column ).Position, Is.EqualTo( 4 ) );
            Assert.That( ( ( AbsolutePositionLocator )clone.Row ).Position, Is.EqualTo( 23 ) );

            Assert.That( clone.ValueFormat.Type, Is.EqualTo( format.ValueFormat.Type ) );
            Assert.That( clone.ValueFormat.Format, Is.EqualTo( format.ValueFormat.Format ) );

            Assert.That( clone.Currency, Is.EqualTo( "Euro" ) );
        }
    }
}
