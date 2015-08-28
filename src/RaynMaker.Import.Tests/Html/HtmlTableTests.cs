using System.Linq;
using NUnit.Framework;
using RaynMaker.Import.Html;

namespace RaynMaker.Import.Tests.Html
{
    [TestFixture]
    [RequiresSTA]
    public class HtmlTableTests : TestBase
    {
        private IHtmlDocument myDocument = null;

        [TestFixtureSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            myDocument = LoadDocument( "ariva.html" );
        }

        [Test]
        public void Creation()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );

            HtmlTable table = myDocument.GetTableByPath( path );

            Assert.AreEqual( "TABLE", table.TableElement.TagName );
            Assert.AreEqual( "TBODY", table.TableBody.TagName );
            Assert.AreEqual( 9, table.Rows.Count() );
        }

        [Test]
        public void GetIndices()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );
            var e = myDocument.GetElementByPath( path );

            Assert.AreEqual( 1, HtmlTable.GetColumnIndex( e ) );
            Assert.AreEqual( 6, HtmlTable.GetRowIndex( e ) );
            Assert.AreEqual( e, HtmlTable.GetEmbeddingTD( e ) );
            Assert.AreEqual( e.Parent, HtmlTable.GetEmbeddingTR( e ) );
        }

        [Test]
        public void GetCellAt()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );
            var e = myDocument.GetElementByPath( path );
            HtmlTable table = myDocument.GetTableByPath( path );

            Assert.AreEqual( e, table.GetCellAt( 6, 1 ) );
        }

        [Test]
        public void GetRow()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );
            var e = myDocument.GetElementByPath( path );

            var row = HtmlTable.GetRow( e );

            Assert.AreEqual( 7, row.Count() );
            Assert.AreEqual( e, row.ElementAt( 1 ) );
        }

        [Test]
        public void GetColumn()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );
            var e = myDocument.GetElementByPath( path );

            var column = HtmlTable.GetColumn( e );

            // Hint: colspan not implemented
            Assert.AreEqual( 8, column.Count() );
            Assert.AreEqual( e, column.ElementAt( 5 ) );
        }
    }
}
