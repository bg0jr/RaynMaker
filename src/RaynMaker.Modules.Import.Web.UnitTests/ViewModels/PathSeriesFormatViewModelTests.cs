using System;
using System.Collections.ObjectModel;
using System.Linq;
using Moq;
using NUnit.Framework;
using RaynMaker.Entities;
using RaynMaker.Entities.Datums;
using RaynMaker.Infrastructure.Services;
using RaynMaker.Modules.Import.Design;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.Modules.Import.Web.ViewModels;
using RaynMaker.SDK.Html;

namespace RaynMaker.Modules.Import.Web.UnitTests.ViewModels
{
    [TestFixture]
    class PathSeriesFormatViewModelTests
    {
        private Mock<IHtmlMarkupBehavior<HtmlTableMarker>> myMarkupBehavior;

        [SetUp]
        public void SetUp()
        {
            myMarkupBehavior = new Mock<IHtmlMarkupBehavior<HtmlTableMarker>> { DefaultValue = DefaultValue.Mock };
        }

        private PathSeriesDescriptorViewModel CreateViewModel( PathSeriesDescriptor descriptor )
        {
            return new PathSeriesDescriptorViewModel( descriptor, myMarkupBehavior.Object );
        }

        [Test]
        public void Ctor_EmptyDescriptor_DescriptorGetsPartiallyInitialized()
        {
            var descriptor = new PathSeriesDescriptor();

            var viewModel = CreateViewModel( descriptor );

            Assert.That( descriptor.ValuesLocator, Is.InstanceOf<StringContainsLocator>() );
            Assert.That( ( (StringContainsLocator)descriptor.ValuesLocator ).HeaderSeriesPosition, Is.EqualTo( -1 ) );
            Assert.That( ( (StringContainsLocator)descriptor.ValuesLocator ).Pattern, Is.Null );
            Assert.That( descriptor.Orientation, Is.EqualTo( SeriesOrientation.Row ) );
            Assert.That( descriptor.Figure, Is.Null );
            Assert.That( descriptor.Path, Is.Null );
            Assert.That( descriptor.TimesLocator, Is.InstanceOf<AbsolutePositionLocator>() );
            Assert.That( ( (AbsolutePositionLocator)descriptor.TimesLocator ).HeaderSeriesPosition, Is.EqualTo( 0 ) );
            Assert.That( ( (AbsolutePositionLocator)descriptor.TimesLocator ).SeriesPosition, Is.EqualTo( -1 ) );
            Assert.That( descriptor.ValueFormat.Type, Is.EqualTo( typeof( double ) ) );
            Assert.That( descriptor.TimeFormat.Type, Is.EqualTo( typeof( int ) ) );
        }

        [Test]
        public void Ctor_ConfiguredDescriptor_DescriptorPropertiesDoNotGetOverwritten()
        {
            var descriptor = new PathSeriesDescriptor();
            descriptor.Figure = "Dividend";
            descriptor.Path = @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]/TBODY[0]";
            descriptor.Orientation = SeriesOrientation.Row;
            descriptor.ValuesLocator = new StringContainsLocator { HeaderSeriesPosition = 7, Pattern = "Dividend in Mio" };
            descriptor.ValueFormat = new FormatColumn( "values", typeof( double ), "00.00" );
            descriptor.TimesLocator = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 0 };
            descriptor.TimeFormat = new FormatColumn( "times", typeof( int ), "000" );
            descriptor.Excludes.Add( 1 );

            var viewModel = CreateViewModel( descriptor );

            Assert.That( descriptor.Figure, Is.EqualTo( "Dividend" ) );
            Assert.That( descriptor.Path, Is.EqualTo( @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]" ) );
            Assert.That( descriptor.Orientation, Is.EqualTo( SeriesOrientation.Row ) );
            Assert.That( ( (StringContainsLocator)descriptor.ValuesLocator ).HeaderSeriesPosition, Is.EqualTo( 7 ) );
            Assert.That( ( (StringContainsLocator)descriptor.ValuesLocator ).Pattern, Is.EqualTo( "Dividend in Mio" ) );
            Assert.That( descriptor.ValueFormat.Type, Is.EqualTo( typeof( double ) ) );
            Assert.That( descriptor.ValueFormat.Format, Is.EqualTo( "00.00" ) );
            Assert.That( ( (AbsolutePositionLocator)descriptor.TimesLocator ).HeaderSeriesPosition, Is.EqualTo( 0 ) );
            Assert.That( ( (AbsolutePositionLocator)descriptor.TimesLocator ).SeriesPosition, Is.EqualTo( 0 ) );
            Assert.That( descriptor.TimeFormat.Type, Is.EqualTo( typeof( int ) ) );
            Assert.That( descriptor.TimeFormat.Format, Is.EqualTo( "000" ) );
            Assert.That( descriptor.Excludes, Is.EquivalentTo( new[] { 1 } ) );
        }

        [Test]
        public void Ctor_ConfiguredDescriptor_ViewModelTakesOverValues()
        {
            var descriptor = new PathSeriesDescriptor();
            descriptor.Figure = "Dividend";
            descriptor.Path = @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]/TBODY[0]";
            descriptor.Orientation = SeriesOrientation.Row;
            descriptor.ValuesLocator = new StringContainsLocator { HeaderSeriesPosition = 7, Pattern = "Dividend in Mio" };
            descriptor.ValueFormat = new FormatColumn( "values", typeof( double ), "00.00" );
            descriptor.TimesLocator = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 3 };
            descriptor.TimeFormat = new FormatColumn( "times", typeof( int ), "000" );
            descriptor.Excludes.Add( 1 );

            var viewModel = CreateViewModel( descriptor );

            Assert.That( viewModel.SelectedDatum, Is.EqualTo( typeof( Dividend ) ) );
            Assert.That( viewModel.Path, Is.EqualTo( @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]" ) );
            Assert.That( viewModel.SelectedOrientation, Is.EqualTo( descriptor.Orientation ) );
            Assert.That( viewModel.ValuesPattern, Is.EqualTo( "Dividend in Mio" ) );
            Assert.That( viewModel.ValuesPosition, Is.EqualTo( 7 ) );
            Assert.That( viewModel.IsValid, Is.False );
            Assert.That( viewModel.ValueFormat, Is.EqualTo( descriptor.ValueFormat ) );
            Assert.That( viewModel.TimesPosition, Is.EqualTo( 3 ) );
            Assert.That( viewModel.TimeFormat, Is.EqualTo( descriptor.TimeFormat ) );
            Assert.That( viewModel.Value, Is.Null.Or.Empty );
        }

        /// <summary>
        /// Ensure that ValuesPosition is -1 in the very beginning so that even if descriptor sets later to 0
        /// this is detected as change which is notified and which updates the marker.
        /// </summary>
        [Test]
        public void Ctor_DescriptorsValuesPositionIsNull_MarkerGotUpdated()
        {
            var descriptor = new PathSeriesDescriptor();
            descriptor.Figure = "Dividend";
            descriptor.Orientation = SeriesOrientation.Row;
            descriptor.ValuesLocator = new StringContainsLocator { HeaderSeriesPosition = 0, Pattern = "column" };

            var viewModel = CreateViewModel( descriptor );

            Assert.That( viewModel.ValuesPosition, Is.EqualTo( 0 ) );
            Assert.That( myMarkupBehavior.Object.Marker.RowHeaderColumn, Is.EqualTo( 0 ) );
        }

        /// <summary>
        /// Ensure that RowPosition is -1 in the very beginning so that even if descriptor sets later to 0
        /// this is detected as change which is notified and which updates the marker.
        /// </summary>
        [Test]
        public void Ctor_DescriptorsRowPositionIsNull_MarkerGotUpdated()
        {
            var descriptor = new PathSeriesDescriptor();
            descriptor.Figure = "Dividend";
            descriptor.Orientation = SeriesOrientation.Row;
            descriptor.TimesLocator = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 0 };

            var viewModel = CreateViewModel( descriptor );

            Assert.That( viewModel.TimesPosition, Is.EqualTo( 0 ) );
            Assert.That( myMarkupBehavior.Object.Marker.ColumnHeaderRow, Is.EqualTo( 0 ) );
        }

        [Test]
        public void Ctor_WhenCalled_FiguresFetchedFromDynamics()
        {
            var descriptor = new PathSeriesDescriptor();

            var viewModel = CreateViewModel( descriptor );

            Assert.That( viewModel.Datums, Is.EquivalentTo( Dynamics.AllDatums ) );
        }

        [Test]
        public void Path_SetToTableCell_PathReducedToTable()
        {
            var descriptor = new PathSeriesDescriptor();
            var viewModel = CreateViewModel( descriptor );

            viewModel.Path = @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]/TBODY[0]/TR[1]/TD[1]";

            Assert.That( viewModel.Path, Is.EqualTo( @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]" ) );
            Assert.That( descriptor.Path, Is.EqualTo( @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]" ) );
        }

        [Test]
        public void Path_WhenCalled_PassedToDescriptor( [Values( null, @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]" )]string path )
        {
            var descriptor = new PathSeriesDescriptor();
            var viewModel = CreateViewModel( descriptor );

            // first set to s.th. different to ensure that really the body of the setter gets executed
            viewModel.Path = "/BODY[0]";

            viewModel.Path = path;

            Assert.That( descriptor.Path, Is.EqualTo( path ) );
        }

        [Test]
        public void Path_PathToSelectedElementNull_SetPathToSelectedElement()
        {
            var descriptor = new PathSeriesDescriptor();
            var viewModel = CreateViewModel( descriptor );

            viewModel.Path = @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]/TBODY[0]/TR[1]/TD[1]";

            myMarkupBehavior.VerifySet( x => x.PathToSelectedElement = @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]" );
        }

        [Test]
        public void Path_PathToSelectedElementNotNull_PathToSelectedElementNotChanged()
        {
            var descriptor = new PathSeriesDescriptor();
            var viewModel = CreateViewModel( descriptor );

            myMarkupBehavior.SetupGet( x => x.PathToSelectedElement ).Returns( @"/TABLE[0]/TBODY[0]/TR[1]/TD[1]" );

            viewModel.Path = @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]/TBODY[0]/TR[1]/TD[1]";

            myMarkupBehavior.VerifySet( x => x.PathToSelectedElement = It.IsAny<string>(), Times.Never );
        }

        /// <summary>
        /// We try to update the selection inside the document according to the state of the ViewModel.
        /// Thats esp. required when toggling around with ViewModels with same Document (only one ViewModel can use a document at the time
        /// because of the HtmlMarkupBehavior).
        /// </summary>
        [Test]
        public void OnDocumentChanged_WhenCalled_PathSetToMarkupBehavior()
        {
            var descriptor = new PathSeriesDescriptor();
            var viewModel = CreateViewModel( descriptor );
            viewModel.Path = "/BODY[0]";

            viewModel.Document = HtmlDocumentExtensions.LoadHtml( "<html><body/></html>" );

            myMarkupBehavior.VerifySet( x => x.PathToSelectedElement = viewModel.Path );
        }

        [Test]
        public void OnSelectionChanged_WhenCalled_PathSet()
        {
            var descriptor = new PathSeriesDescriptor();

            var viewModel = CreateViewModel( descriptor );
            viewModel.Document = new Mock<IHtmlDocument>().Object;

            myMarkupBehavior.SetupGet( x => x.SelectedElement ).Returns( new Mock<IHtmlElement>().Object );
            myMarkupBehavior.SetupGet( x => x.PathToSelectedElement ).Returns( "/BODY[0]/DIV[7]" );

            myMarkupBehavior.Raise( x => x.SelectionChanged += null, this, EventArgs.Empty );

            Assert.That( viewModel.Path, Is.EqualTo( "/BODY[0]/DIV[7]" ) );
        }
    }
}
