using System;
using System.Runtime.Serialization;
using NUnit.Framework;
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
                : base( "dummy", columns )
            {
            }
        }

        [Test]
        public void SkipRowsIsImmutable()
        {
            var descriptor = new DummyDescriptor(
                new FormatColumn( "c1", typeof( double ), "0.00" ),
                new FormatColumn( "c2", typeof( string ), "" ) );

            var skipRows = new int[] { 1, 2, 3 };
            descriptor.SkipRows = skipRows;

            skipRows[ 1 ] = 42;

            Assert.AreEqual( 1, descriptor.SkipRows[ 0 ] );
            Assert.AreEqual( 2, descriptor.SkipRows[ 1 ] );
            Assert.AreEqual( 3, descriptor.SkipRows[ 2 ] );
        }

        [Test]
        public void SkipColumnsIsImmutable()
        {
            var descriptor = new DummyDescriptor(
                new FormatColumn( "c1", typeof( double ), "0.00" ),
                new FormatColumn( "c2", typeof( string ), "" ) );

            var skipColumns = new int[] { 1, 2, 3 };
            descriptor.SkipColumns = skipColumns;

            skipColumns[ 1 ] = 42;

            Assert.AreEqual( 1, descriptor.SkipColumns[ 0 ] );
            Assert.AreEqual( 2, descriptor.SkipColumns[ 1 ] );
            Assert.AreEqual( 3, descriptor.SkipColumns[ 2 ] );
        }

        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var descriptor = new DummyDescriptor(
                new FormatColumn( "c1", typeof( double ), "0.00" ),
                new FormatColumn( "c2", typeof( string ), "" ) );
            descriptor.SkipRows = new[] { 5, 9 };
            descriptor.SkipColumns = new[] { 11, 88 };

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
