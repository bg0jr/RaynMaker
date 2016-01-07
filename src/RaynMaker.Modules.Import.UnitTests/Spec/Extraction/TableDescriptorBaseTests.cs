using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using NUnit.Framework;
using Plainion.Collections;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.SDK;

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
                foreach( var col in columns )
                {
                    Columns.Add( col );
                }
            }
        }

        [Test]
        public void Columns_Add_ValueAdded()
        {
            var descriptor = new DummyDescriptor();

            descriptor.Columns.Add( new FormatColumn( "c1", typeof( double ), "0.00" ) );

            Assert.That( descriptor.Columns[ 0 ].Format, Is.EqualTo( "0.00" ) );
        }

        [Test]
        public void Columns_Add_ChangeIsNotified()
        {
            var descriptor = new DummyDescriptor();
            var counter = new CollectionChangedCounter( descriptor.Columns );

            descriptor.Columns.Add( new FormatColumn( "c1", typeof( double ), "0.00" ) );

            Assert.That( counter.Count, Is.EqualTo( 1 ) );
        }

        [Test]
        public void SkipRows_Add_ValueAdded()
        {
            var descriptor = new DummyDescriptor();

            descriptor.SkipRows.Add( 11 );

            Assert.That( descriptor.SkipRows, Contains.Item( 11 ) );
        }

        [Test]
        public void SkipRows_Add_ChangeIsNotified()
        {
            var descriptor = new DummyDescriptor();
            var counter = new CollectionChangedCounter( descriptor.SkipRows );

            descriptor.SkipRows.Add( 11 );

            Assert.That( counter.Count, Is.EqualTo( 1 ) );
        }

        [Test]
        public void SkipColumns_Add_ValueAdded()
        {
            var descriptor = new DummyDescriptor();

            descriptor.SkipColumns.Add( 11 );

            Assert.That( descriptor.SkipColumns, Contains.Item( 11 ) );
        }

        [Test]
        public void SkipColumns_Add_ChangeIsNotified()
        {
            var descriptor = new DummyDescriptor();
            var counter = new CollectionChangedCounter( descriptor.SkipColumns );

            descriptor.SkipColumns.Add( 11 );

            Assert.That( counter.Count, Is.EqualTo( 1 ) );
        }

        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var descriptor = new DummyDescriptor(
                new FormatColumn( "c1", typeof( double ), "0.00" ),
                new FormatColumn( "c2", typeof( string ), "" ) );
            descriptor.SkipRows.AddRange( 5, 9 );
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
        public void Clone_WhenCalled_CollectionsAreMutableAndObservable()
        {
            var descriptor = new DummyDescriptor();

            var clone = FigureDescriptorFactory.Clone( descriptor );

            var counter = new CollectionChangedCounter( clone.Columns );
            clone.Columns.Add( new FormatColumn() );
            Assert.That( counter.Count, Is.EqualTo( 1 ) );

            counter = new CollectionChangedCounter( clone.SkipColumns );
            clone.SkipColumns.Add( 1 );
            Assert.That( counter.Count, Is.EqualTo( 1 ) );

            counter = new CollectionChangedCounter( clone.SkipRows );
            clone.SkipRows.Add( 1 );
            Assert.That( counter.Count, Is.EqualTo( 1 ) );
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var descriptor = new DummyDescriptor(
                new FormatColumn( "c1", typeof( double ), "0.00" ),
                new FormatColumn( "c2", typeof( string ), "" ) );
            descriptor.Figure = "Test";

            RecursiveValidator.Validate( descriptor );
        }

        [Test]
        public void Validate_MissingCollumns_Throws()
        {
            var descriptor = new DummyDescriptor();
            descriptor.Figure = "Test";

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( descriptor ) );
            Assert.That( ex.Message, Is.StringContaining( "Columns must not be empty" ) );
        }

        [Test]
        public void Validate_InvalidColumn_Throws()
        {
            var descriptor = new DummyDescriptor();
            descriptor.Figure = "Equity";
            descriptor.Columns.Add( new FormatColumn() );

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( descriptor ) );
            Assert.That( ex.Message, Is.StringContaining( "Type field is required" ) );
        }
    }
}
