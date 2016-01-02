using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Extraction
{
    [TestFixture]
    public class PathSeriesDescriptorTests
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var descriptor = new PathSeriesDescriptor(  );
            descriptor.Path = "123";

            var clone = FigureDescriptorFactory.Clone( descriptor );

            Assert.That( clone.Path, Is.EqualTo( "123" ) );
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var descriptor = new PathSeriesDescriptor(  );
            descriptor.Path = "123";
            descriptor.ValuesLocator = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 4 };
            descriptor.ValueFormat = new FormatColumn( "values", typeof( double ), "0.00" );

            RecursiveValidator.Validate( descriptor );
        }

        [Test]
        public void Validate_InvalidPath_Throws( [Values( null, "" )]string path )
        {
            var descriptor = new PathSeriesDescriptor();
            descriptor.Path = path;
            descriptor.ValuesLocator = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 4 };
            descriptor.ValueFormat = new FormatColumn( "values", typeof( double ), "0.00" );

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( descriptor ) );
            Assert.That( ex.Message, Is.StringContaining( "The Path field is required" ) );
        }
    }
}
