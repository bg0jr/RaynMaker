using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Moq;
using NUnit.Framework;
using RaynMaker.Infrastructure.Services;
using RaynMaker.Modules.Import.Design;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Documents.WinForms;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.Modules.Import.Web.ViewModels;
using RaynMaker.SDK.Html;

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
        public void DefineCellInHtmlTable()
        {
            var descriptor = new PathCellDescriptor();
            descriptor.Figure = "Price";

            var doc = (HtmlDocumentAdapter)LoadDocument<IHtmlDocument>( "Html", "ariva.prices.DE0007664039.html" );

            var viewModel = new PathCellDescriptorViewModel( myLutService.Object, descriptor );
            viewModel.Document = doc;

            HtmlMarkupAutomationProvider.SimulateClickOn( doc.Document, "rym_FrakfurtPrice" );

            Assert.That( descriptor.Path, Is.EqualTo( @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]/TBODY[0]" ) );
            Assert.That( viewModel.Value, Is.EqualTo( "134,356\t€" ) );
            Assert.That( ( (StringContainsLocator)descriptor.Column ).HeaderSeriesPosition, Is.EqualTo( 0 ) );
            Assert.That( ( (StringContainsLocator)descriptor.Column ).Pattern, Is.EqualTo( "Letzter" ) );
            Assert.That( ( (StringContainsLocator)descriptor.Row ).HeaderSeriesPosition, Is.EqualTo( 0 ) );
            Assert.That( ( (StringContainsLocator)descriptor.Row ).Pattern, Is.EqualTo( "Frankfurt" ) );

            viewModel.ValueFormat.Type = typeof( double );
            viewModel.ValueFormat.Format = "00,00";
            viewModel.ValueFormat.ExtractionPattern = new Regex( @"([0-9,\.]+)" );

            Assert.That( viewModel.Value, Is.EqualTo( ( 134.356 ).ToString() ) );

            viewModel.SelectedCurrency = myLutService.Object.CurrenciesLut.Currencies.Single( c => c.Symbol == "EUR" );

            Assert.That( descriptor.Currency, Is.EqualTo( "EUR" ) );

            var selectedCell = doc.GetElementById( "rym_FrakfurtPrice" );

            Assert.That( HtmlMarkupAutomationProvider.IsMarked( selectedCell ), Is.True );

            // now that we have defined a FigureDescriptor lets toggle the document and check
            // that descriptor remains untouched and Html table cell gets correctly reselected

            viewModel.Document = null;

            Assert.That( HtmlMarkupAutomationProvider.IsMarked( selectedCell ), Is.False );

            viewModel.Document = doc;

            //doc.Document.Title = TestContext.CurrentContext.Test.Name;
            //doc.Document.OpenDocumentInExternalBrowser();

            Assert.That( descriptor.Path, Is.EqualTo( @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]/TBODY[0]" ) );
            Assert.That( ( (StringContainsLocator)descriptor.Column ).HeaderSeriesPosition, Is.EqualTo( 0 ) );
            Assert.That( ( (StringContainsLocator)descriptor.Column ).Pattern, Is.EqualTo( "Letzter" ) );
            Assert.That( ( (StringContainsLocator)descriptor.Row ).HeaderSeriesPosition, Is.EqualTo( 0 ) );
            Assert.That( ( (StringContainsLocator)descriptor.Row ).Pattern, Is.EqualTo( "Frankfurt" ) );

            Assert.That( descriptor.ValueFormat.ExtractionPattern.ToString(), Is.EqualTo( @"([0-9,\.]+)" ) );

            Assert.That( viewModel.Value, Is.EqualTo( ( 134.356 ).ToString() ) );
            Assert.That( descriptor.Currency, Is.EqualTo( "EUR" ) );

            Assert.That( HtmlMarkupAutomationProvider.IsMarked( selectedCell ), Is.True );
        }

        [Test]
        public void DefineSeriesInHtmlTable()
        {
            var descriptor = new PathSeriesDescriptor();
            descriptor.Figure = "Dividend";
            descriptor.Orientation = SeriesOrientation.Row;

            var doc = (HtmlDocumentAdapter)LoadDocument<IHtmlDocument>( "Html", "ariva.fundamentals.DE0005190003.html" );

            var viewModel = new PathSeriesDescriptorViewModel( descriptor );
            viewModel.Document = doc;

            HtmlMarkupAutomationProvider.SimulateClickOn( doc.Document, "rym_Dividend" );

            Assert.That( descriptor.Path, Is.EqualTo( @"/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]" ) );
            // we cannot calculate the exact clicked position form descriptor (anyway does not matter for series).
            // -> we just select the first value which is not header
            Assert.That( viewModel.Value, Is.EqualTo( "350,0" ) );
            Assert.That( ( (StringContainsLocator)descriptor.ValuesLocator ).HeaderSeriesPosition, Is.EqualTo( 0 ) );
            Assert.That( ( (StringContainsLocator)descriptor.ValuesLocator ).Pattern, Is.EqualTo( "Dividendenausschüttung in Mio." ) );
            Assert.That( ( (AbsolutePositionLocator)descriptor.TimesLocator ).HeaderSeriesPosition, Is.EqualTo( 0 ) );
            Assert.That( ( (AbsolutePositionLocator)descriptor.TimesLocator ).SeriesPosition, Is.EqualTo( 0 ) );

            viewModel.ValueFormat.Type = typeof( double );
            viewModel.ValueFormat.Format = "00,0";
            viewModel.ValueFormat.InMillions = true;

            viewModel.TimesPosition = 1;
            
            Assert.That( viewModel.Value, Is.EqualTo( ( 350000000d ).ToString() ) );

            var selectedCell = doc.GetElementById( "rym_Dividend" );

            Assert.That( HtmlMarkupAutomationProvider.IsMarked( selectedCell ), Is.True );

            // now that we have defined a FigureDescriptor lets toggle the document and check
            // that descriptor remains untouched and Html table cell gets correctly reselected

            viewModel.Document = null;

            Assert.That( HtmlMarkupAutomationProvider.IsMarked( selectedCell ), Is.False );

            viewModel.Document = doc;

            //doc.Document.Title = TestContext.CurrentContext.Test.Name;
            //doc.Document.OpenDocumentInExternalBrowser();

            Assert.That( descriptor.Path, Is.EqualTo( @"/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]" ) );
            Assert.That( ( (StringContainsLocator)descriptor.ValuesLocator ).HeaderSeriesPosition, Is.EqualTo( 0 ) );
            Assert.That( ( (StringContainsLocator)descriptor.ValuesLocator ).Pattern, Is.EqualTo( "Dividendenausschüttung in Mio." ) );
            Assert.That( ( (AbsolutePositionLocator)descriptor.TimesLocator ).HeaderSeriesPosition, Is.EqualTo( 0 ) );
            Assert.That( ( (AbsolutePositionLocator)descriptor.TimesLocator ).SeriesPosition, Is.EqualTo( 1 ) );

            Assert.That( descriptor.ValueFormat.InMillions, Is.True );

            Assert.That( viewModel.Value, Is.EqualTo( ( 350000000d ).ToString() ) );

            Assert.That( HtmlMarkupAutomationProvider.IsMarked( selectedCell ), Is.True );
        }
    }
}
