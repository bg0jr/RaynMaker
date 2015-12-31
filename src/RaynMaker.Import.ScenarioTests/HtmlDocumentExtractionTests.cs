using System.Text.RegularExpressions;
using NUnit.Framework;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Spec.v2.Extraction;

namespace RaynMaker.Import.ScenarioTests
{
    [TestFixture]
    [RequiresSTA]
    public class HtmlDocumentExtractionTests : TestBase
    {
        [Test]
        public void GetSingleValueByPath()
        {
            var doc = LoadDocument<IHtmlDocument>( "ariva.overview.US0138171014.html" );

            var descriptor = new PathSingleValueDescriptor( "Ariva.Wpkn" );
            descriptor.Path = @"/BODY[0]/DIV[4]/DIV[0]/DIV[3]/DIV[0]";
            descriptor.ValueFormat = new ValueFormat( typeof( int ), "00000000", new Regex( @"WKN: (\d+)" ) );

            var parser = DocumentProcessorsFactory.CreateParser( doc, descriptor );
            var table = parser.ExtractTable();

            Assert.AreEqual( 1, table.Rows.Count );

            Assert.AreEqual( 850206, table.Rows[ 0 ][ 0 ] );
        }

        [Test]
        public void GetSeriesByPath()
        {
            var doc = LoadDocument<IHtmlDocument>( "ariva.html" );

            var descriptor = new PathSeriesDescriptor( "Eps" );
            descriptor.Path = @"/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]";
            descriptor.Anchor = TableFragmentDescriptor.ForRow( new StringContainsLocator( 0, "verwässertes Ergebnis pro Aktie" ) );

            descriptor.SeriesNamePosition = 0;
            descriptor.TimeAxisPosition = 1;

            descriptor.SkipColumns = new[] { 0 };

            descriptor.ValueFormat = new FormatColumn( "value", typeof( float ), "00,00" );
            descriptor.TimeAxisFormat = new FormatColumn( "year", typeof( int ), "00000000" );

            var parser = DocumentProcessorsFactory.CreateParser( doc, descriptor );
            var table = parser.ExtractTable();

            Assert.AreEqual( 6, table.Rows.Count );

            Assert.AreEqual( 2.78f, table.Rows[ 0 ][ 0 ] );
            Assert.AreEqual( 3.00f, table.Rows[ 1 ][ 0 ] );
            Assert.AreEqual( 2.89f, table.Rows[ 2 ][ 0 ] );
            Assert.AreEqual( 3.30f, table.Rows[ 3 ][ 0 ] );
            Assert.AreEqual( 3.33f, table.Rows[ 4 ][ 0 ] );
            Assert.AreEqual( 4.38f, table.Rows[ 5 ][ 0 ] );

            Assert.AreEqual( 2001, table.Rows[ 0 ][ 1 ] );
            Assert.AreEqual( 2002, table.Rows[ 1 ][ 1 ] );
            Assert.AreEqual( 2003, table.Rows[ 2 ][ 1 ] );
            Assert.AreEqual( 2004, table.Rows[ 3 ][ 1 ] );
            Assert.AreEqual( 2005, table.Rows[ 4 ][ 1 ] );
            Assert.AreEqual( 2006, table.Rows[ 5 ][ 1 ] );
        }
    }
}
