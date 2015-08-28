using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Tests.Core
{
    [TestFixture]
    public class HtmlDocumentTest : TestBase
    {
        private IDocumentBrowser myBrowser = null;

        [SetUp]
        public void SetUp()
        {
            myBrowser = DocumentBrowserFactory.Create();
        }

        [TearDown]
        public void TearDown()
        {
            myBrowser = null;
        }

        [Test]
        public void WpknFromAriva()
        {
            var inputFile = Path.Combine( TestDataRoot, "Recognition", "Core", "ariva.overview.US0138171014.html" );
            var doc = myBrowser.GetDocument( new Navigation( DocumentType.Html, inputFile ) );

            var format = new PathSingleValueFormat( "Ariva.Wpkn" );
            format.Path = @"/BODY[0]/DIV[4]/DIV[0]/DIV[3]/DIV[0]";
            format.ValueFormat = new ValueFormat( typeof( int ), "00000000", new Regex( @"WKN: (\d+)" ) );

            var table = doc.ExtractTable( format );

            Assert.AreEqual( 1, table.Rows.Count );

            Assert.AreEqual( 850206, table.Rows[ 0 ][ 0 ] );
        }

    }
}
