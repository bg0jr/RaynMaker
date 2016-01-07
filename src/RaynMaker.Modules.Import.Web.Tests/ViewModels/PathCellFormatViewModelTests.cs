using System.Collections.ObjectModel;
using System.Linq;
using Moq;
using NUnit.Framework;
using RaynMaker.Entities;
using RaynMaker.Entities.Datums;
using RaynMaker.Infrastructure.Services;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.Modules.Import.Web.ViewModels;

namespace RaynMaker.Modules.Import.Web.UnitTests.ViewModels
{
    [TestFixture]
    class PathCellFormatViewModelTests
    {
        private Mock<ILutService> myLutService;

        [SetUp]
        public void SetUp()
        {
            myLutService = new Mock<ILutService> { DefaultValue = DefaultValue.Mock };
            Mock.Get( myLutService.Object.CurrenciesLut ).SetupGet( x => x.Currencies ).Returns( new ObservableCollection<Entities.Currency>() );
            myLutService.Object.CurrenciesLut.Currencies.Add( new Entities.Currency { Symbol = "EUR" } );
        }

        [Test]
        public void Ctor_EmptyDescriptor_DescriptorGetsPartiallyInitialized()
        {
            var descriptor = new PathCellDescriptor();

            var viewModel = new PathCellFormatViewModel( myLutService.Object, descriptor );

            Assert.That( descriptor.Column, Is.InstanceOf<StringContainsLocator>() );
            Assert.That( ( ( StringContainsLocator )descriptor.Column ).HeaderSeriesPosition, Is.EqualTo( -1 ) );
            Assert.That( ( ( StringContainsLocator )descriptor.Column ).Pattern, Is.Null );
            Assert.That( descriptor.Currency, Is.Null );
            Assert.That( descriptor.Figure, Is.Null );
            Assert.That( descriptor.Path, Is.Null );
            Assert.That( descriptor.Row, Is.InstanceOf<StringContainsLocator>() );
            Assert.That( ( ( StringContainsLocator )descriptor.Row ).HeaderSeriesPosition, Is.EqualTo( -1 ) );
            Assert.That( ( ( StringContainsLocator )descriptor.Row ).Pattern, Is.Null );
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

            var viewModel = new PathCellFormatViewModel( myLutService.Object, descriptor );

            Assert.That( ( ( StringContainsLocator )descriptor.Column ).HeaderSeriesPosition, Is.EqualTo( 7 ) );
            Assert.That( ( ( StringContainsLocator )descriptor.Column ).Pattern, Is.EqualTo( "column" ) );
            Assert.That( descriptor.Currency, Is.EqualTo( "EUR" ) );
            Assert.That( descriptor.Figure, Is.EqualTo( "Dividend" ) );
            Assert.That( descriptor.Path, Is.EqualTo( @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]/TBODY[0]" ) );
            Assert.That( ( ( StringContainsLocator )descriptor.Row ).HeaderSeriesPosition, Is.EqualTo( 4 ) );
            Assert.That( ( ( StringContainsLocator )descriptor.Row ).Pattern, Is.EqualTo( "row" ) );
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

            var viewModel = new PathCellFormatViewModel( myLutService.Object, descriptor );

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

        [Test]
        public void Ctor_WhenCalled_CurrenciesAreTakenFromLutServiceAndIncludesNull()
        {
            var descriptor = new PathCellDescriptor();

            var viewModel = new PathCellFormatViewModel( myLutService.Object, descriptor );

            // "null" is included to allow the user to select "nothing"
            Assert.That( viewModel.Currencies, Is.EquivalentTo( new Entities.Currency[] { null }.Concat( myLutService.Object.CurrenciesLut.Currencies ) ) );
        }

        [Test]
        public void Ctor_WhenCalled_FiguresFetchedFromDynamics()
        {
            var descriptor = new PathCellDescriptor();

            var viewModel = new PathCellFormatViewModel( myLutService.Object, descriptor );

            Assert.That( viewModel.Datums, Is.EquivalentTo( Dynamics.AllDatums ) );
        }

        [Test]
        public void Path_SetToTableCell_PathReducedToTable()
        {
            var descriptor = new PathCellDescriptor();
            var viewModel = new PathCellFormatViewModel( myLutService.Object, descriptor );

            viewModel.Path = @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]/TBODY[0]/TR[1]/TD[1]";

            Assert.That( viewModel.Path, Is.EqualTo( @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]/TBODY[0]" ) );
            Assert.That( descriptor.Path, Is.EqualTo( @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]/TBODY[0]" ) );
        }
    }
}
