using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Parsers;
using RaynMaker.Import.Parsers.Html;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Tests.Parsers
{
    [TestFixture]
    [RequiresSTA]
    public class HtmlParserTests : TestBase
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
            var inputFile = Path.Combine( TestDataRoot, "Core", "ariva.overview.US0138171014.html" );
            myBrowser.Navigate( new DocumentLocator( DocumentType.Html, inputFile ) );

            var format = new PathSingleValueFormat( "Ariva.Wpkn" );
            format.Path = @"/BODY[0]/DIV[4]/DIV[0]/DIV[3]/DIV[0]";
            format.ValueFormat = new ValueFormat( typeof( int ), "00000000", new Regex( @"WKN: (\d+)" ) );

            var parser = new HtmlParser( ( HtmlDocumentHandle )myBrowser.Document, format );
            var table = parser.ExtractTable();

            Assert.AreEqual( 1, table.Rows.Count );

            Assert.AreEqual( 850206, table.Rows[ 0 ][ 0 ] );
        }
    }
}
