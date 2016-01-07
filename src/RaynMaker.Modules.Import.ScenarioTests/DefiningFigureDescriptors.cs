using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Moq;
using NUnit.Framework;
using RaynMaker.Entities;
using RaynMaker.Entities.Datums;
using RaynMaker.Infrastructure.Services;
using RaynMaker.Modules.Import.Design;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Documents.WinForms;
using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.Modules.Import.Web.ViewModels;

namespace RaynMaker.Modules.Import.ScenarioTests
{
    [TestFixture]
    [RequiresSTA]
    class DefiningFigureDescriptors : TestBase
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
        public void DefineCell()
        {
            var descriptor = new PathCellDescriptor();
            descriptor.Figure = "Price";

            var doc = ( HtmlDocumentAdapter )LoadDocument<IHtmlDocument>( "Html", "ariva.prices.DE0007664039.html" );

            var viewModel = new PathCellFormatViewModel( myLutService.Object, descriptor );
            viewModel.Document = doc;

            HtmlMarkupAutomationProvider.SimulateClickOn( doc.Document, "rym_FrakfurtPrice" );

            Assert.That( descriptor.Path, Is.EqualTo( @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]/TBODY[0]" ) );
            Assert.That( viewModel.Value, Is.EqualTo( "134,356\t€" ) );
            Assert.That( ( ( StringContainsLocator )descriptor.Column ).HeaderSeriesPosition, Is.EqualTo( 0 ) );
            Assert.That( ( ( StringContainsLocator )descriptor.Column ).Pattern, Is.EqualTo( "Letzter" ) );
            Assert.That( ( ( StringContainsLocator )descriptor.Row ).HeaderSeriesPosition, Is.EqualTo( 0 ) );
            Assert.That( ( ( StringContainsLocator )descriptor.Row ).Pattern, Is.EqualTo( "Frankfurt" ) );

            viewModel.ValueFormat.Type = typeof( double );
            viewModel.ValueFormat.Format = "00,00";
            viewModel.ValueFormat.ExtractionPattern = new Regex( @"([0-9,\.]+)" );

            Assert.That( viewModel.Value, Is.EqualTo( ( 134.356 ).ToString() ) );

            viewModel.SelectedCurrency = myLutService.Object.CurrenciesLut.Currencies.Single( c => c.Symbol == "EUR" );

            Assert.That( descriptor.Currency, Is.EqualTo( "EUR" ) );

            var selectedCell = doc.GetElementById( "rym_FrakfurtPrice" );

            Assert.That( HtmlMarkupAutomationProvider.IsMarked( selectedCell ), Is.True );
        }
    }
}
