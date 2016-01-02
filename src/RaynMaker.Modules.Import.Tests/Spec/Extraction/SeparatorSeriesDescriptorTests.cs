using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Parsers;
using RaynMaker.Modules.Import.Parsers.Text;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Extraction
{
    [TestFixture]
    public class SeparatorSeriesDescriptorTests
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var descriptor = new SeparatorSeriesDescriptor( "dummy" );
            descriptor.Separator = "#";

            var clone = FigureDescriptorFactory.Clone( descriptor );

            Assert.That( clone.Separator, Is.EqualTo( "#" ) );
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var descriptor = new SeparatorSeriesDescriptor( "dummy" );
            descriptor.Separator = "#";
            descriptor.ValuesLocator = new AbsolutePositionLocator( 0, 4 );
            descriptor.ValueFormat = new FormatColumn( "values", typeof( double ), "0.00" );

            RecursiveValidator.Validate( descriptor );
        }

        [Test]
        public void Validate_InvalidSeparator_Throws( [Values( null, "" )]string seperator )
        {
            var descriptor = new SeparatorSeriesDescriptor( "dummy" );
            descriptor.Separator = seperator;
            descriptor.ValuesLocator = new AbsolutePositionLocator( 0, 4 );
            descriptor.ValueFormat = new FormatColumn( "values", typeof( double ), "0.00" );

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( descriptor ) );
            Assert.That( ex.Message, Is.StringContaining( "The Separator field is required" ) );
        }
    }
}
