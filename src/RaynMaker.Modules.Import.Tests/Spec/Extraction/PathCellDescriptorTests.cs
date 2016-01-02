using System;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using Plainion.Validation;
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
            var descriptor = new PathCellDescriptor( "dummy" );
            descriptor.Path = "123";
            descriptor.Column = new AbsolutePositionLocator(0, 4 );
            descriptor.Row = new AbsolutePositionLocator( 0, 23 );
            descriptor.ValueFormat = new ValueFormat( typeof( double ), "0.00" );
            descriptor.Currency = "Euro";

            var clone = FormatFactory.Clone( descriptor );

            Assert.That( clone.Path, Is.EqualTo( "123" ) );

            Assert.That( ( ( AbsolutePositionLocator )clone.Column ).SeriesPosition, Is.EqualTo( 4 ) );
            Assert.That( ( ( AbsolutePositionLocator )clone.Row ).SeriesPosition, Is.EqualTo( 23 ) );

            Assert.That( clone.ValueFormat.Type, Is.EqualTo( descriptor.ValueFormat.Type ) );
            Assert.That( clone.ValueFormat.Format, Is.EqualTo( descriptor.ValueFormat.Format ) );

            Assert.That( clone.Currency, Is.EqualTo( "Euro" ) );
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var descriptor = new PathCellDescriptor( "dummy" );
            descriptor.Path = "123";
            descriptor.Column = new AbsolutePositionLocator( 0, 4 );
            descriptor.Row = new AbsolutePositionLocator( 0, 23 );
            descriptor.ValueFormat = new ValueFormat( typeof( double ), "0.00" );

            RecursiveValidator.Validate( descriptor );
        }

        [Test]
        public void Validate_InvalidPath_Throws( [Values( null, "" )]string path )
        {
            var descriptor = new PathCellDescriptor( "dummy" );
            descriptor.Path = path;
            descriptor.Column = new AbsolutePositionLocator( 0, 4 );
            descriptor.Row = new AbsolutePositionLocator( 0, 23 );
            descriptor.ValueFormat = new ValueFormat( typeof( double ), "0.00" );

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( descriptor ) );
            Assert.That( ex.Message, Is.StringContaining( "The Path field is required" ) );
        }

        [Test]
        public void Validate_MisingColumn_Throws()
        {
            var descriptor = new PathCellDescriptor( "dummy" );
            descriptor.Path = "123";
            descriptor.Column = null;
            descriptor.Row = new AbsolutePositionLocator( 0, 23 );
            descriptor.ValueFormat = new ValueFormat( typeof( double ), "0.00" );

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( descriptor ) );
            Assert.That( ex.Message, Is.StringContaining( "The Column field is required" ) );
        }

        [Test]
        public void Validate_MisingRow_Throws()
        {
            var descriptor = new PathCellDescriptor( "dummy" );
            descriptor.Path = "123";
            descriptor.Column = new AbsolutePositionLocator( 0, 4 );
            descriptor.Row = null;
            descriptor.ValueFormat = new ValueFormat( typeof( double ), "0.00" );

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( descriptor ) );
            Assert.That( ex.Message, Is.StringContaining( "The Row field is required" ) );
        }

        [Test]
        public void Validate_MisingValueFormat_Throws()
        {
            var descriptor = new PathCellDescriptor( "dummy" );
            descriptor.Path = "123";
            descriptor.Column = new AbsolutePositionLocator( 0, 4 );
            descriptor.Row = new AbsolutePositionLocator( 0, 23 );
            descriptor.ValueFormat = null;

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( descriptor ) );
            Assert.That( ex.Message, Is.StringContaining( "The ValueFormat field is required" ) );
        }
    }
}
