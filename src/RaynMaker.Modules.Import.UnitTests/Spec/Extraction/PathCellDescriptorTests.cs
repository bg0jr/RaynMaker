using System;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.SDK;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Extraction
{
    [TestFixture]
    public class PathCellDescriptorTests
    {
        [Test]
        public void Path_Set_ValueIsSet()
        {
            var descriptor = new PathCellDescriptor();

            descriptor.Path = "123";

            Assert.That( descriptor.Path, Is.EqualTo( "123" ) );
        }

        [Test]
        public void Path_Set_ChangeIsNotified()
        {
            var descriptor = new PathCellDescriptor();
            var counter = new PropertyChangedCounter( descriptor );

            descriptor.Path = "123";

            Assert.That( counter.GetCount( () => descriptor.Path ), Is.EqualTo( 1 ) );
        }

        [Test]
        public void Column_Set_ValueIsSet()
        {
            var descriptor = new PathCellDescriptor();

            descriptor.Column = new AbsolutePositionLocator { HeaderSeriesPosition = 7, SeriesPosition = 4 };

            Assert.That( descriptor.Column.HeaderSeriesPosition, Is.EqualTo( 7 ) );
        }

        [Test]
        public void Column_Set_ChangeIsNotified()
        {
            var descriptor = new PathCellDescriptor();
            var counter = new PropertyChangedCounter( descriptor );

            descriptor.Column = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 4 };

            Assert.That( counter.GetCount( () => descriptor.Column ), Is.EqualTo( 1 ) );
        }

        [Test]
        public void Row_Set_ValueIsSet()
        {
            var descriptor = new PathCellDescriptor();

            descriptor.Row = new AbsolutePositionLocator { HeaderSeriesPosition = 7, SeriesPosition = 4 };

            Assert.That( descriptor.Row.HeaderSeriesPosition, Is.EqualTo( 7 ) );
        }

        [Test]
        public void Row_Set_ChangeIsNotified()
        {
            var descriptor = new PathCellDescriptor();
            var counter = new PropertyChangedCounter( descriptor );

            descriptor.Row = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 4 };

            Assert.That( counter.GetCount( () => descriptor.Row ), Is.EqualTo( 1 ) );
        }

        [Test]
        public void ValueFormat_Set_ValueIsSet()
        {
            var descriptor = new PathCellDescriptor();

            descriptor.ValueFormat = new ValueFormat( typeof( double ), "0.00" );

            Assert.That( descriptor.ValueFormat.Format, Is.EqualTo( "0.00" ) );
        }

        [Test]
        public void ValueFormat_Set_ChangeIsNotified()
        {
            var descriptor = new PathCellDescriptor();
            var counter = new PropertyChangedCounter( descriptor );

            descriptor.ValueFormat = new ValueFormat( typeof( double ), "0.00" );

            Assert.That( counter.GetCount( () => descriptor.ValueFormat ), Is.EqualTo( 1 ) );
        }

        [Test]
        public void Currency_Set_ValueIsSet()
        {
            var descriptor = new PathCellDescriptor();

            descriptor.Currency = "Euro";

            Assert.That( descriptor.Currency, Is.EqualTo( "Euro" ) );
        }

        [Test]
        public void Currency_Set_ChangeIsNotified()
        {
            var descriptor = new PathCellDescriptor();
            var counter = new PropertyChangedCounter( descriptor );

            descriptor.Currency = "Euro";

            Assert.That( counter.GetCount( () => descriptor.Currency ), Is.EqualTo( 1 ) );
        }

        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var descriptor = new PathCellDescriptor();
            descriptor.Path = "123";
            descriptor.Column = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 4 };
            descriptor.Row = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 23 };
            descriptor.ValueFormat = new ValueFormat( typeof( double ), "0.00" );
            descriptor.Currency = "Euro";

            var clone = FigureDescriptorFactory.Clone( descriptor );

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
            var descriptor = new PathCellDescriptor();
            descriptor.Figure = "Price";
            descriptor.Path = "123";
            descriptor.Column = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 4 };
            descriptor.Row = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 23 };
            descriptor.ValueFormat = new ValueFormat( typeof( double ), "0.00" );

            RecursiveValidator.Validate( descriptor );
        }

        [Test]
        public void Validate_InvalidPath_Throws( [Values( null, "" )]string path )
        {
            var descriptor = new PathCellDescriptor();
            descriptor.Figure = "Price";
            descriptor.Path = path;
            descriptor.Column = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 4 };
            descriptor.Row = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 23 };
            descriptor.ValueFormat = new ValueFormat( typeof( double ), "0.00" );

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( descriptor ) );
            Assert.That( ex.Message, Is.StringContaining( "The Path field is required" ) );
        }

        [Test]
        public void Validate_MisingColumn_Throws()
        {
            var descriptor = new PathCellDescriptor();
            descriptor.Figure = "Price";
            descriptor.Path = "123";
            descriptor.Column = null;
            descriptor.Row = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 23 };
            descriptor.ValueFormat = new ValueFormat( typeof( double ), "0.00" );

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( descriptor ) );
            Assert.That( ex.Message, Is.StringContaining( "The Column field is required" ) );
        }

        [Test]
        public void Validate_MisingRow_Throws()
        {
            var descriptor = new PathCellDescriptor();
            descriptor.Figure = "Price";
            descriptor.Path = "123";
            descriptor.Column = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 4 };
            descriptor.Row = null;
            descriptor.ValueFormat = new ValueFormat( typeof( double ), "0.00" );

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( descriptor ) );
            Assert.That( ex.Message, Is.StringContaining( "The Row field is required" ) );
        }

        [Test]
        public void Validate_MisingValueFormat_Throws()
        {
            var descriptor = new PathCellDescriptor();
            descriptor.Figure = "Price";
            descriptor.Path = "123";
            descriptor.Column = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 4 };
            descriptor.Row = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 23 };
            descriptor.ValueFormat = null;

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( descriptor ) );
            Assert.That( ex.Message, Is.StringContaining( "The ValueFormat field is required" ) );
        }

        [Test]
        public void Validate_InvalidColumn_Throws()
        {
            var descriptor = new PathCellDescriptor();
            descriptor.Figure = "Price";
            descriptor.Path = "123";
            descriptor.Column = new AbsolutePositionLocator();
            descriptor.Row = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 23 };
            descriptor.ValueFormat = new ValueFormat( typeof( double ), "0.00" );

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( descriptor ) );
            Assert.That( ex.Message, Is.StringContaining( "HeaderSeriesPosition must be between 0 and " + int.MaxValue ) );
        }

        [Test]
        public void Validate_InvalidRow_Throws()
        {
            var descriptor = new PathCellDescriptor();
            descriptor.Figure = "Price";
            descriptor.Path = "123";
            descriptor.Column = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 23 };
            descriptor.Row = new AbsolutePositionLocator();
            descriptor.ValueFormat = new ValueFormat( typeof( double ), "0.00" );

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( descriptor ) );
            Assert.That( ex.Message, Is.StringContaining( "HeaderSeriesPosition must be between 0 and " + int.MaxValue ) );
        }

        [Test]
        public void Validate_InvalidValueFormat_Throws()
        {
            var descriptor = new PathCellDescriptor();
            descriptor.Figure = "Price";
            descriptor.Path = "123";
            descriptor.Column = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 23 };
            descriptor.Row = new AbsolutePositionLocator();
            descriptor.ValueFormat = new ValueFormat( );

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( descriptor ) );
            Assert.That( ex.Message, Is.StringContaining( "Type field is required" ) );
        }
    }
}
