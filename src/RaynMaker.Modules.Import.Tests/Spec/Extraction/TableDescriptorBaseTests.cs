using System;
using System.Runtime.Serialization;
using NUnit.Framework;
using Plainion.Collections;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Extraction
{
    [TestFixture]
    public class TableDescriptorBaseTests
    {
        [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "DummyDescriptor" )]
        private class DummyDescriptor : TableDescriptorBase
        {
            public DummyDescriptor( params FormatColumn[] columns )
            {
                foreach(var col in columns)
                {
                    Columns.Add( col );
                }
            }
        }

        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var descriptor = new DummyDescriptor(
                new FormatColumn( "c1", typeof( double ), "0.00" ),
                new FormatColumn( "c2", typeof( string ), "" ) );
            descriptor.SkipRows.AddRange(  5, 9 );
            descriptor.SkipColumns.AddRange( 11, 88 );

            var clone = FigureDescriptorFactory.Clone( descriptor );

            Assert.That( clone.Columns[ 0 ].Name, Is.EqualTo( "c1" ) );
            Assert.That( clone.Columns[ 0 ].Type, Is.EqualTo( typeof( double ) ) );
            Assert.That( clone.Columns[ 0 ].Format, Is.EqualTo( "0.00" ) );

            Assert.That( clone.Columns[ 1 ].Name, Is.EqualTo( "c2" ) );
            Assert.That( clone.Columns[ 1 ].Type, Is.EqualTo( typeof( string ) ) );
            Assert.That( clone.Columns[ 1 ].Format, Is.EqualTo( "" ) );

            Assert.That( clone.SkipRows, Is.EquivalentTo( descriptor.SkipRows ) );
            Assert.That( clone.SkipColumns, Is.EquivalentTo( descriptor.SkipColumns ) );
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var descriptor = new DummyDescriptor(
                new FormatColumn( "c1", typeof( double ), "0.00" ),
                new FormatColumn( "c2", typeof( string ), "" ) );

            RecursiveValidator.Validate( descriptor );
        }

        [Test]
        public void Ctor_MissingColumns_Throws()
        {
            var ex = Assert.Throws<ArgumentException>( () => new DummyDescriptor() );

            Assert.That( ex.Message, Is.StringContaining( "Collection must not be empty: columns" ) );
        }
    }
}
