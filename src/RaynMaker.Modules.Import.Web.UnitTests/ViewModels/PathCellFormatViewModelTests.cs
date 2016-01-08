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

namespace RaynMaker.Modules.Import.Web.UnitTests.ViewModels
{
    [TestFixture]
    class PathCellFormatViewModelTests
    {
        private Mock<ILutService> myLutService;
        private Mock<IHtmlMarkupBehavior<HtmlTableMarker>> myMarkupBehavior;

        [SetUp]
        public void SetUp()
        {
            myLutService = new Mock<ILutService> { DefaultValue = DefaultValue.Mock };
            Mock.Get( myLutService.Object.CurrenciesLut ).SetupGet( x => x.Currencies ).Returns( new ObservableCollection<Entities.Currency>() );
            myLutService.Object.CurrenciesLut.Currencies.Add( new Entities.Currency { Symbol = "EUR" } );

            myMarkupBehavior = new Mock<IHtmlMarkupBehavior<HtmlTableMarker>> { DefaultValue = DefaultValue.Mock };
        }

        private PathCellFormatViewModel CreateViewModel( PathCellDescriptor descriptor )
        {
            return new PathCellFormatViewModel( myLutService.Object, descriptor, myMarkupBehavior.Object );
        }

        [Test]
        public void Ctor_EmptyDescriptor_DescriptorGetsPartiallyInitialized()
        {
            var descriptor = new PathCellDescriptor();

            var viewModel = CreateViewModel( descriptor );

            Assert.That( descriptor.Column, Is.InstanceOf<StringContainsLocator>() );
            Assert.That( ( (StringContainsLocator)descriptor.Column ).HeaderSeriesPosition, Is.EqualTo( -1 ) );
            Assert.That( ( (StringContainsLocator)descriptor.Column ).Pattern, Is.Null );
            Assert.That( descriptor.Currency, Is.Null );
            Assert.That( descriptor.Figure, Is.Null );
            Assert.That( descriptor.Path, Is.Null );
            Assert.That( descriptor.Row, Is.InstanceOf<StringContainsLocator>() );
            Assert.That( ( (StringContainsLocator)descriptor.Row ).HeaderSeriesPosition, Is.EqualTo( -1 ) );
            Assert.That( ( (StringContainsLocator)descriptor.Row ).Pattern, Is.Null );
            Assert.That( descriptor.ValueFormat.Type, Is.EqualTo( typeof( double ) ) );
        }

        [Test]
        public void Ctor_ConfiguredDescriptor_DescriptorPropertiesDoNotGetOverwritten()
        {
            var descriptor = new PathCellDescriptor();
            descriptor.Column = new StringContainsLocator { HeaderSeriesPosition = 7, Pattern = "column" };
            descriptor.Currency = "EUR";
            descriptor.Figure = "Dividend";
            descriptor.Path = @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]/TBODY[0]";
            descriptor.Row = new StringContainsLocator { HeaderSeriesPosition = 4, Pattern = "row" };
            descriptor.ValueFormat = new ValueFormat( typeof( double ), "00.00" );

            var viewModel = CreateViewModel( descriptor );

            Assert.That( ( (StringContainsLocator)descriptor.Column ).HeaderSeriesPosition, Is.EqualTo( 7 ) );
            Assert.That( ( (StringContainsLocator)descriptor.Column ).Pattern, Is.EqualTo( "column" ) );
            Assert.That( descriptor.Currency, Is.EqualTo( "EUR" ) );
            Assert.That( descriptor.Figure, Is.EqualTo( "Dividend" ) );
            Assert.That( descriptor.Path, Is.EqualTo( @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]/TBODY[0]" ) );
            Assert.That( ( (StringContainsLocator)descriptor.Row ).HeaderSeriesPosition, Is.EqualTo( 4 ) );
            Assert.That( ( (StringContainsLocator)descriptor.Row ).Pattern, Is.EqualTo( "row" ) );
            Assert.That( descriptor.ValueFormat.Type, Is.EqualTo( typeof( double ) ) );
            Assert.That( descriptor.ValueFormat.Format, Is.EqualTo( "00.00" ) );
        }

        [Test]
        public void Ctor_ConfiguredDescriptor_ViewModelTakesOverValues()
        {
            var descriptor = new PathCellDescriptor();
            descriptor.Column = new StringContainsLocator { HeaderSeriesPosition = 7, Pattern = "column" };
            descriptor.Currency = "EUR";
            descriptor.Figure = "Dividend";
            descriptor.Path = @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]/TBODY[0]";
            descriptor.Row = new StringContainsLocator { HeaderSeriesPosition = 4, Pattern = "row" };
            descriptor.ValueFormat = new ValueFormat( typeof( double ), "00.00" );

            var viewModel = CreateViewModel( descriptor );

            Assert.That( viewModel.ColumnPattern, Is.EqualTo( "column" ) );
            Assert.That( viewModel.ColumnPosition, Is.EqualTo( 7 ) );
            Assert.That( viewModel.IsColumnValid, Is.False );
            Assert.That( viewModel.IsRowValid, Is.False );
            Assert.That( viewModel.Path, Is.EqualTo( @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]/TBODY[0]" ) );
            Assert.That( viewModel.RowPattern, Is.EqualTo( "row" ) );
            Assert.That( viewModel.RowPosition, Is.EqualTo( 4 ) );
            Assert.That( viewModel.SelectedCurrency.Symbol, Is.EqualTo( "EUR" ) );
            Assert.That( viewModel.SelectedDatum, Is.EqualTo( typeof( Dividend ) ) );
            Assert.That( viewModel.Value, Is.Null.Or.Empty );
            Assert.That( viewModel.ValueFormat, Is.EqualTo( descriptor.ValueFormat ) );
        }

        /// <summary>
        /// Ensure that ColumnPosition is -1 in the very beginning so that even if descriptor sets later to 0
        /// this is detected as change which is notified and which updates the marker.
        /// </summary>
        [Test]
        public void Ctor_DescriptorsColumnPositionIsNull_MarkerGotUpdated()
        {
            var descriptor = new PathCellDescriptor();
            descriptor.Figure = "Dividend";
            descriptor.Column = new StringContainsLocator { HeaderSeriesPosition = 0, Pattern = "column" };

            var viewModel = CreateViewModel( descriptor );

            Assert.That( viewModel.ColumnPosition, Is.EqualTo( 0 ) );
            Assert.That( myMarkupBehavior.Object.Marker.ColumnHeaderRow, Is.EqualTo( 0 ) );
        }

        /// <summary>
        /// Ensure that RowPosition is -1 in the very beginning so that even if descriptor sets later to 0
        /// this is detected as change which is notified and which updates the marker.
        /// </summary>
        [Test]
        public void Ctor_DescriptorsRowPositionIsNull_MarkerGotUpdated()
        {
            var descriptor = new PathCellDescriptor();
            descriptor.Figure = "Dividend";
            descriptor.Row = new StringContainsLocator { HeaderSeriesPosition = 0, Pattern = "row" };

            var viewModel = CreateViewModel( descriptor );

            Assert.That( viewModel.RowPosition, Is.EqualTo( 0 ) );
            Assert.That( myMarkupBehavior.Object.Marker.RowHeaderColumn, Is.EqualTo( 0 ) );
        }

        [Test]
        public void Ctor_WhenCalled_CurrenciesAreTakenFromLutServiceAndIncludesNull()
        {
            var descriptor = new PathCellDescriptor();

            var viewModel = CreateViewModel( descriptor );

            // "null" is included to allow the user to select "nothing"
            Assert.That( viewModel.Currencies, Is.EquivalentTo( new Entities.Currency[] { null }.Concat( myLutService.Object.CurrenciesLut.Currencies ) ) );
        }

        [Test]
        public void Ctor_WhenCalled_FiguresFetchedFromDynamics()
        {
            var descriptor = new PathCellDescriptor();

            var viewModel = CreateViewModel( descriptor );

            Assert.That( viewModel.Datums, Is.EquivalentTo( Dynamics.AllDatums ) );
        }

        [Test]
        public void Path_SetToTableCell_PathReducedToTable()
        {
            var descriptor = new PathCellDescriptor();
            var viewModel = CreateViewModel( descriptor );

            viewModel.Path = @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]/TBODY[0]/TR[1]/TD[1]";

            Assert.That( viewModel.Path, Is.EqualTo( @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]/TBODY[0]" ) );
            Assert.That( descriptor.Path, Is.EqualTo( @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]/TBODY[0]" ) );
        }

        [Test]
        public void Path_WhenCalled_PassedToDescriptor( [Values( null, @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]/TBODY[0]" )]string path )
        {
            var descriptor = new PathCellDescriptor();
            var viewModel = CreateViewModel( descriptor );

            // first set to s.th. different to ensure that really the body of the setter gets executed
            viewModel.Path = "/BODY[0]";

            viewModel.Path = path;

            Assert.That( descriptor.Path, Is.EqualTo( path ) );
        }

        [Test]
        public void Path_PathToSelectedElementNull_SetPathToSelectedElement()
        {
            var descriptor = new PathCellDescriptor();
            var viewModel = CreateViewModel( descriptor );

            viewModel.Path = @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]/TBODY[0]/TR[1]/TD[1]";

            myMarkupBehavior.VerifySet( x => x.PathToSelectedElement = @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]/TBODY[0]" );
        }

        [Test]
        public void Path_PathToSelectedElementNotNull_PathToSelectedElementNotChanged()
        {
            var descriptor = new PathCellDescriptor();
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
            var descriptor = new PathCellDescriptor();
            var viewModel = CreateViewModel( descriptor );
            viewModel.Path = "/BODY[0]";

            viewModel.Document = new Mock<IHtmlDocument>().Object;

            myMarkupBehavior.VerifySet( x => x.PathToSelectedElement = viewModel.Path );
        }

        [Test]
        public void OnSelectionChanged_WhenCalled_PathSet()
        {
            var descriptor = new PathCellDescriptor();

            var viewModel = CreateViewModel( descriptor );
            viewModel.Document = new Mock<IHtmlDocument>().Object;

            myMarkupBehavior.SetupGet( x => x.SelectedElement ).Returns( new Mock<IHtmlElement>().Object );
            myMarkupBehavior.SetupGet( x => x.PathToSelectedElement ).Returns( "/BODY[0]/DIV[7]" );

            myMarkupBehavior.Raise( x => x.SelectionChanged += null, this, EventArgs.Empty );

            Assert.That( viewModel.Path, Is.EqualTo( "/BODY[0]/DIV[7]" ) );
        }
    }
}
