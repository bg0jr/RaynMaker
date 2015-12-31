using System;
using System.Runtime.Serialization;
using NUnit.Framework;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2.Extraction;

namespace RaynMaker.Import.Tests.Spec.Extraction
{
    [TestFixture]
    public class AbstractSeriesFormatTests
    {
        [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "DummyFormat" )]
        private class DummyFormat : AbstractSeriesDescriptor
        {
            public DummyFormat()
                : base( "dummy" )
            {
            }
        }

        [Test]
        public void SkipValuesIsImmutable()
        {
            var format = new DummyFormat();

            var skipValues = new int[] { 1, 2, 3 };
            format.SkipValues = skipValues;

            skipValues[ 1 ] = 42;

            Assert.AreEqual( 1, format.SkipValues[ 0 ] );
            Assert.AreEqual( 2, format.SkipValues[ 1 ] );
            Assert.AreEqual( 3, format.SkipValues[ 2 ] );
        }

        [Test]
        public void SkipColumnsIsImmutable()
        {
            var format = new DummyFormat();

            var skipColumns = new int[] { 1, 2, 3 };
            format.SkipValues = skipColumns;

            skipColumns[ 1 ] = 42;

            Assert.AreEqual( 1, format.SkipValues[ 0 ] );
            Assert.AreEqual( 2, format.SkipValues[ 1 ] );
            Assert.AreEqual( 3, format.SkipValues[ 2 ] );
        }
        
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var format = new DummyFormat();
            format.Anchor = TableFragmentDescriptor.ForCell( new AbsolutePositionLocator( 4 ), new AbsolutePositionLocator( 8 ) );
            format.TimeAxisPosition = 23;
            format.ValueFormat = new FormatColumn( "value", typeof( double ), "0.00" );
            format.TimeFormat = new FormatColumn( "time", typeof( DateTime ), "G" );
            format.SkipValues = new[] { 11, 88 };

            var clone = FormatFactory.Clone( format );

            Assert.That( clone.TimeAxisPosition, Is.EqualTo( format.TimeAxisPosition ) );

            Assert.That( clone.ValueFormat.Name, Is.EqualTo( format.ValueFormat.Name ) );
            Assert.That( clone.ValueFormat.Type, Is.EqualTo( format.ValueFormat.Type ) );
            Assert.That( clone.ValueFormat.Format, Is.EqualTo( format.ValueFormat.Format ) );

            Assert.That( clone.TimeFormat.Name, Is.EqualTo( format.TimeFormat.Name ) );
            Assert.That( clone.TimeFormat.Type, Is.EqualTo( format.TimeFormat.Type ) );
            Assert.That( clone.TimeFormat.Format, Is.EqualTo( format.TimeFormat.Format ) );

            Assert.That( ( ( AbsolutePositionLocator )clone.Anchor.Row ).Position, Is.EqualTo( 4 ) );
            Assert.That( ( ( AbsolutePositionLocator )clone.Anchor.Column ).Position, Is.EqualTo( 8 ) );
        
            Assert.That( clone.SkipValues, Is.EquivalentTo( format.SkipValues ) );
        }
    }
}
