using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Extraction
{
    [TestFixture]
    public class SeriesDescriptorBaseTests
    {
        [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "DummyDescriptor" )]
        private class DummyDescriptor : SeriesDescriptorBase
        {
            public DummyDescriptor()
                : base( "dummy" )
            {
            }
        }

        [Test]
        public void SkipValuesIsImmutable()
        {
            var descriptor = new DummyDescriptor();

            var excludes = new int[] { 1, 2, 3 };
            descriptor.Excludes = excludes;

            excludes[ 1 ] = 42;

            Assert.AreEqual( 1, descriptor.Excludes[ 0 ] );
            Assert.AreEqual( 2, descriptor.Excludes[ 1 ] );
            Assert.AreEqual( 3, descriptor.Excludes[ 2 ] );
        }

        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var descriptor = new DummyDescriptor();

            descriptor.Orientation = SeriesOrientation.Row;

            descriptor.ValuesLocator = new AbsolutePositionLocator( 0, 4 );
            descriptor.ValueFormat = new FormatColumn( "value", typeof( double ), "0.00" );

            descriptor.TimesLocator = new AbsolutePositionLocator( 0, 23 );
            descriptor.TimeFormat = new FormatColumn( "time", typeof( DateTime ), "G" );

            descriptor.Excludes = new[] { 11, 88 };

            var clone = FigureDescriptorFactory.Clone( descriptor );

            Assert.That( clone.Orientation, Is.EqualTo( descriptor.Orientation ) );

            Assert.That( ( ( AbsolutePositionLocator )clone.ValuesLocator ).SeriesPosition, Is.EqualTo( 4 ) );
            Assert.That( clone.ValueFormat.Name, Is.EqualTo( descriptor.ValueFormat.Name ) );
            Assert.That( clone.ValueFormat.Type, Is.EqualTo( descriptor.ValueFormat.Type ) );
            Assert.That( clone.ValueFormat.Format, Is.EqualTo( descriptor.ValueFormat.Format ) );

            Assert.That( ( ( AbsolutePositionLocator )clone.TimesLocator ).SeriesPosition, Is.EqualTo( 23 ) );
            Assert.That( clone.TimeFormat.Name, Is.EqualTo( descriptor.TimeFormat.Name ) );
            Assert.That( clone.TimeFormat.Type, Is.EqualTo( descriptor.TimeFormat.Type ) );
            Assert.That( clone.TimeFormat.Format, Is.EqualTo( descriptor.TimeFormat.Format ) );

            Assert.That( clone.Excludes, Is.EquivalentTo( descriptor.Excludes ) );
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var descriptor = new DummyDescriptor();
            descriptor.ValuesLocator = new AbsolutePositionLocator( 0, 4 );
            descriptor.ValueFormat = new FormatColumn( "values", typeof( double ), "0.00" );

            RecursiveValidator.Validate( descriptor );
        }

        [Test]
        public void Validate_MissingValueLocator_Throws()
        {
            var descriptor = new SeparatorSeriesDescriptor( "dummy" );
            descriptor.ValuesLocator = null;
            descriptor.ValueFormat = new FormatColumn( "values", typeof( double ), "0.00" );

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( descriptor ) );
            Assert.That( ex.Message, Is.StringContaining( "The ValuesLocator field is required" ) );
        }

        [Test]
        public void Validate_MissingValueFormat_Throws()
        {
            var descriptor = new SeparatorSeriesDescriptor( "dummy" );
            descriptor.ValuesLocator = new AbsolutePositionLocator( 0, 4 );
            descriptor.ValueFormat = null;

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( descriptor ) );
            Assert.That( ex.Message, Is.StringContaining( "The ValueFormat field is required" ) );
        }
    }
}
