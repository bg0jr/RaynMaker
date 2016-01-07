using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.SDK;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Extraction
{
    [TestFixture]
    public class SeparatorSeriesDescriptorTests
    {
        [Test]
        public void Separator_Set_ValueIsSet()
        {
            var descriptor = new SeparatorSeriesDescriptor();

            descriptor.Separator = ";";

            Assert.That( descriptor.Separator, Is.EqualTo( ";" ) );
        }

        [Test]
        public void Separator_Set_ChangeIsNotified()
        {
            var descriptor = new SeparatorSeriesDescriptor();
            var counter = new PropertyChangedCounter( descriptor );

            descriptor.Separator = ";";

            Assert.That( counter.GetCount( () => descriptor.Separator ), Is.EqualTo( 1 ) );
        }

        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var descriptor = new SeparatorSeriesDescriptor();
            descriptor.Separator = "#";

            var clone = FigureDescriptorFactory.Clone( descriptor );

            Assert.That( clone.Separator, Is.EqualTo( "#" ) );
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var descriptor = new SeparatorSeriesDescriptor();
            descriptor.Figure = "Dept";
            descriptor.Separator = "#";
            descriptor.ValuesLocator = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 4 };
            descriptor.ValueFormat = new FormatColumn( "values", typeof( double ), "0.00" );

            RecursiveValidator.Validate( descriptor );
        }

        [Test]
        public void Validate_InvalidSeparator_Throws( [Values( null, "" )]string seperator )
        {
            var descriptor = new SeparatorSeriesDescriptor();
            descriptor.Figure = "Dept";
            descriptor.Separator = seperator;
            descriptor.ValuesLocator = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 4 };
            descriptor.ValueFormat = new FormatColumn( "values", typeof( double ), "0.00" );

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( descriptor ) );
            Assert.That( ex.Message, Is.StringContaining( "The Separator field is required" ) );
        }
    }
}
