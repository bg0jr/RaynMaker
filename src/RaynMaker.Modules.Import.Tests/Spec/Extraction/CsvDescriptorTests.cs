using System;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Extraction
{
    [TestFixture]
    public class CsvDescriptorTests
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var descriptor = new CsvDescriptor( "dummy", ";",
                new FormatColumn( "c1", typeof( double ), "0.00" ),
                new FormatColumn( "c2", typeof( string ), "" ) );

            var clone = FormatFactory.Clone( descriptor );

            Assert.That( clone.Separator, Is.EqualTo( ";" ) );

            Assert.That( clone.Columns[ 0 ].Name, Is.EqualTo( "c1" ) );
            Assert.That( clone.Columns[ 1 ].Name, Is.EqualTo( "c2" ) );
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var descriptor = new CsvDescriptor( "dummy", ";",
                new FormatColumn( "c1", typeof( double ), "0.00" ),
                new FormatColumn( "c2", typeof( string ), "" ) );

            RecursiveValidator.Validate( descriptor );
        }

        [Test]
        public void Ctor_InvalidSeparator_Throws( [Values( null, "" )]string separator )
        {
            var ex = Assert.Throws<ArgumentNullException>( () =>  new CsvDescriptor( "dummy", separator,
                new FormatColumn( "c1", typeof( double ), "0.00" ),
                new FormatColumn( "c2", typeof( string ), "" ) ));

            Assert.That( ex.Message, Is.StringContaining( "string must not null or empty: sep" ) );
        }
    }
}
