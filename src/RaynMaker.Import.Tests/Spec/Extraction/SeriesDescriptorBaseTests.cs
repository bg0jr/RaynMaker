using System;
using System.Runtime.Serialization;
using NUnit.Framework;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2.Extraction;

namespace RaynMaker.Import.Tests.Spec.Extraction
{
    [TestFixture]
    public class SeriesDescriptorBaseTests
    {
        [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "DummyFormat" )]
        private class DummyFormat : SeriesDescriptorBase
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

            var excludes = new int[] { 1, 2, 3 };
            format.Excludes = excludes;

            excludes[ 1 ] = 42;

            Assert.AreEqual( 1, format.Excludes[ 0 ] );
            Assert.AreEqual( 2, format.Excludes[ 1 ] );
            Assert.AreEqual( 3, format.Excludes[ 2 ] );
        }

        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var format = new DummyFormat();

            format.Orientation = SeriesOrientation.Row;

            format.ValuesLocator = new AbsolutePositionLocator( 4 );
            format.ValueFormat = new FormatColumn( "value", typeof( double ), "0.00" );

            format.TimesLocator = new AbsolutePositionLocator( 23 );
            format.TimeFormat = new FormatColumn( "time", typeof( DateTime ), "G" );

            format.Excludes = new[] { 11, 88 };

            var clone = FormatFactory.Clone( format );

            Assert.That( clone.Orientation, Is.EqualTo( format.Orientation ) );
            
            Assert.That( ((AbsolutePositionLocator)clone.ValuesLocator).Position, Is.EqualTo( 4 ) );
            Assert.That( clone.ValueFormat.Name, Is.EqualTo( format.ValueFormat.Name ) );
            Assert.That( clone.ValueFormat.Type, Is.EqualTo( format.ValueFormat.Type ) );
            Assert.That( clone.ValueFormat.Format, Is.EqualTo( format.ValueFormat.Format ) );

            Assert.That( ( ( AbsolutePositionLocator )clone.TimesLocator ).Position, Is.EqualTo( 23 ) );
            Assert.That( clone.TimeFormat.Name, Is.EqualTo( format.TimeFormat.Name ) );
            Assert.That( clone.TimeFormat.Type, Is.EqualTo( format.TimeFormat.Type ) );
            Assert.That( clone.TimeFormat.Format, Is.EqualTo( format.TimeFormat.Format ) );

            Assert.That( clone.Excludes, Is.EquivalentTo( format.Excludes ) );
        }
    }
}
