using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Parsers;
using RaynMaker.Import.Parsers.Html;
using RaynMaker.Import.Spec.v2;
using RaynMaker.Import.Spec.v2.Extraction;
using RaynMaker.Import.Spec.v2.Locating;

namespace RaynMaker.Import.ScenarioTests
{
    [TestFixture]
    [RequiresSTA]
    public class HtmlDocumentExtractionTests : TestBase
    {
        private IDocumentBrowser myBrowser = null;

        [SetUp]
        public void SetUp()
        {
            myBrowser = DocumentProcessorsFactory.CreateBrowser();
        }

        [TearDown]
        public void TearDown()
        {
            myBrowser = null;
        }

        [Test]
        public void WpknFromAriva()
        {
            var inputFile = Path.Combine( TestDataRoot, "ariva.overview.US0138171014.html" );
            myBrowser.Navigate( DocumentType.Html, new DocumentLocator( inputFile ) );

            var descriptor = new PathSingleValueDescriptor( "Ariva.Wpkn" );
            descriptor.Path = @"/BODY[0]/DIV[4]/DIV[0]/DIV[3]/DIV[0]";
            descriptor.ValueFormat = new ValueFormat( typeof( int ), "00000000", new Regex( @"WKN: (\d+)" ) );

            var parser = DocumentProcessorsFactory.CreateParser( myBrowser.Document, descriptor );
            var table = parser.ExtractTable();

            Assert.AreEqual( 1, table.Rows.Count );

            Assert.AreEqual( 850206, table.Rows[ 0 ][ 0 ] );
        }

        [Test]
        public void ExtractStockOverviewFromYahoo()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/CENTER[0]/P[1]/TABLE[0]/TBODY[0]/TR[0]/TD[0]/TABLE[3]/TBODY[0]/TR[0]/TD[0]/CENTER[0]/TABLE[3]/TBODY[0]/TR[0]/TD[0]/TABLE[0]" );

            var doc = LoadDocument( "yahoo-bmw-all.html" );
            var result = doc.ExtractTable( path, false );

            Assert.IsTrue( result.Success );

            var table = result.Value;
            table.Dump();

            Assert.AreEqual( 9, table.Rows.Count );

            foreach( DataRow row in table.Rows.ToSet().Skip( 1 ) )
            {
                string symbolLink = ( ( IHtmlElement )row[ 0 ] ).FirstLinkOrInnerText();
                // sample: "http://de.finance.yahoo.com/q?s=DTE.SG"
                Match m = Regex.Match( symbolLink, @"s=([a-zA-Z0-9]+)\.([A-Za-z]+)$" );
                Assert.IsTrue( m.Success );
            }
        }
    }
}
