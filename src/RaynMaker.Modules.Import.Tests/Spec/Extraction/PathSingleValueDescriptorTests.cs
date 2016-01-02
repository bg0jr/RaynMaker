using System;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Extraction
{
    [TestFixture]
    public class PathSingleValueDescriptorTests
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var descriptor = new PathSingleValueDescriptor();
            descriptor.Path = "111";
            descriptor.ValueFormat = new ValueFormat( typeof( int ), "0.xx" );

            var clone = FigureDescriptorFactory.Clone( descriptor );

            Assert.That( clone.Path, Is.EqualTo( "111" ) );
            Assert.That( clone.ValueFormat.Format, Is.EqualTo( "0.xx" ) );
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var descriptor = new PathSingleValueDescriptor();
            descriptor.Path = "123";
            descriptor.ValueFormat = new FormatColumn( "values", typeof( double ), "0.00" );

            RecursiveValidator.Validate( descriptor );
        }

        [Test]
        public void Validate_InvalidPath_Throws( [Values( null, "" )]string path )
        {
            var descriptor = new PathSingleValueDescriptor();
            descriptor.Path = path;
            descriptor.ValueFormat = new FormatColumn( "values", typeof( double ), "0.00" );

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( descriptor ) );
            Assert.That( ex.Message, Is.StringContaining( "The Path field is required" ) );
        }

        [Test]
        public void Validate_MisingValueFormat_Throws()
        {
            var descriptor = new PathSingleValueDescriptor();
            descriptor.Path = "123";
            descriptor.ValueFormat = null;

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( descriptor ) );
            Assert.That( ex.Message, Is.StringContaining( "The ValueFormat field is required" ) );
        }
    }
}
