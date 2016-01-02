using System;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Extraction
{
    [TestFixture]
    public class FormatColumnTests
    {
        [Test]
        public void Ctor_WhenCalled_NameIsSet()
        {
            var col = new FormatColumn( "test", typeof( string ) );

            Assert.That( col.Name, Is.EqualTo( "test" ) );
        }

        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var col = new FormatColumn( "c1", typeof( string ) );

            var clone = FigureDescriptorFactory.Clone( col );

            Assert.That( clone.Name, Is.EqualTo( "c1" ) );
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var col = new FormatColumn( "c1", typeof( string ) );

            RecursiveValidator.Validate( col );
        }

        [Test]
        public void Ctor_InvalidColumnName_Throws( [Values( null, "" )]string columnName )
        {
            var ex = Assert.Throws<ArgumentNullException>( () => new FormatColumn( columnName, typeof( string ) ) );

            Assert.That( ex.Message, Is.StringContaining( "string must not null or empty: name" ) );
        }
    }
}
