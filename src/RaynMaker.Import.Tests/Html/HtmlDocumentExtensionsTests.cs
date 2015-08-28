using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Blade.Data;
using NUnit.Framework;
using RaynMaker.Import.Html;

namespace RaynMaker.Import.Tests.Html
{
    [TestFixture]
    [RequiresSTA]
    public class HtmlDocumentExtensionsTests : TestBase
    {
        [Test]
        public void ExtractEpsFromAriva()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );

            TableExtractionSettings settings = new TableExtractionSettings();
            settings.RowHeaderColumn = 0;
            settings.ColumnHeaderRow = 1;
            settings.Dimension = CellDimension.Row;
            settings.SeriesName = "verwässertes Ergebnis pro Aktie";
            settings.SeriesHeaderType = typeof( int );
            settings.SeriesValueType = typeof( float );

            var doc = LoadDocument( "ariva.html" );
            var result = doc.ExtractTable( path, settings, new HtmlExtractionSettings() );

            Assert.IsTrue( result.Success );

            var table = result.Value;
            table.Dump();

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

            foreach ( DataRow row in table.Rows.ToSet().Skip( 1 ) )
            {
                string symbolLink = ((IHtmlElement)row[ 0 ]).FirstLinkOrInnerText();
                // sample: "http://de.finance.yahoo.com/q?s=DTE.SG"
                Match m = Regex.Match( symbolLink, @"s=([a-zA-Z0-9]+)\.([A-Za-z]+)$" );
                Assert.IsTrue( m.Success );
            }
        }

        [Test]
        public void GetElementByPath()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );
            var doc = LoadDocument( "ariva.html" );
            var e = doc.GetElementByPath( path );

            Assert.AreEqual( "TD", e.TagName );
            Assert.AreEqual( "TR", e.Parent.TagName );
            Assert.AreEqual( 1, e.GetChildPos() );
            Assert.AreEqual( 6, e.Parent.GetChildPos() );
        }

        [Test]
        public void GetTextByPath()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );

            var doc = LoadDocument( "ariva.html" );
            var e = doc.GetElementByPath( path );
            string value = doc.GetTextByPath( path );

            Assert.AreEqual( e.InnerText, value );
            Assert.AreEqual( "2,78", value );
        }

        [Test]
        public void GetTextByPath_SimplePath()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[0]/DIV[1]" );

            var doc = LoadDocument( "ariva.html" );
            var e = doc.GetElementByPath( path );
            string value = doc.GetTextByPath( path );

            Assert.AreEqual( "Willkommen, Gast!", value );
        }

        [Test]
        public void GetTableByPath()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );

            var doc = LoadDocument( "ariva.html" );
            var e = doc.GetElementByPath( path );
            e = e.Parent.Parent.Parent;
            HtmlTable table = doc.GetTableByPath( path );

            Assert.AreEqual( e, table.TableElement );
            Assert.AreEqual( e.Children.First(), table.TableBody );
        }

        [Test]
        public void GetFormByName()
        {
            var doc = LoadDocument( "ariva.historicalprices.DE0008404005.html" );
            var form = doc.GetFormByName( "histcsv" );

            Assert.That( form, Is.Not.Null );
            Assert.That( form.Name, Is.EqualTo( "histcsv" ).IgnoreCase );
        }

        // TODO:
        // - test single cell
        // - test column as series
        // - test other sites
    }
}
