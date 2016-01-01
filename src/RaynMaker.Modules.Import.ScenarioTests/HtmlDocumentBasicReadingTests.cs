using System.Linq;
using NUnit.Framework;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Parsers;
using RaynMaker.Modules.Import.Parsers.Html;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.ScenarioTests
{
    [TestFixture]
    [RequiresSTA]
    public class HtmlDocumentBasicReadingTests : TestBase
    {
        private IHtmlDocument myDocument = null;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            myDocument = LoadDocument<IHtmlDocument>( "ariva.html" );
        }

        [Test]
        public void Table_Create()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );

            HtmlTable table = myDocument.GetTableByPath( path );

            Assert.AreEqual( "TABLE", table.TableElement.TagName );
            Assert.AreEqual( "TBODY", table.TableBody.TagName );
            Assert.AreEqual( 9, table.Rows.Count() );
        }

        [Test]
        public void Table_GetIndices()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );
            
            var e = myDocument.GetElementByPath( path );

            Assert.AreEqual( 1, HtmlTable.GetColumnIndex( e ) );
            Assert.AreEqual( 6, HtmlTable.GetRowIndex( e ) );
            Assert.AreEqual( e, HtmlTable.GetEmbeddingTD( e ) );
            Assert.AreEqual( e.Parent, HtmlTable.GetEmbeddingTR( e ) );
        }

        [Test]
        public void Table_GetCellAt()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );
            var e = myDocument.GetElementByPath( path );
            HtmlTable table = myDocument.GetTableByPath( path );

            Assert.AreEqual( e, table.GetCellAt( 6, 1 ) );
        }

        [Test]
        public void Table_GetRow()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );
            var e = myDocument.GetElementByPath( path );

            var row = HtmlTable.GetRow( e );

            Assert.AreEqual( 7, row.Count() );
            Assert.AreEqual( e, row.ElementAt( 1 ) );
        }

        [Test]
        public void Table_GetColumn()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );
            var e = myDocument.GetElementByPath( path );

            var column = HtmlTable.GetColumn( e );

            // Hint: colspan not implemented
            Assert.AreEqual( 8, column.Count() );
            Assert.AreEqual( e, column.ElementAt( 5 ) );
        }

        [Test]
        public void GetRoot()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );

            var e = myDocument.GetElementByPath( path );

            Assert.AreEqual( "TD", e.TagName );
            Assert.AreEqual( "HTML", e.GetRoot().TagName );
        }

        [Test]
        public void GetPath()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );

            var e = myDocument.GetElementByPath( path );

            Assert.AreEqual( path.ToString(), e.GetPath().ToString() );
        }

        [Test]
        public void GetChildPos()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );

            var e = myDocument.GetElementByPath( path );

            Assert.AreEqual( 1, e.GetChildPos() );
        }

        [Test]
        public void GetChildPosRoot()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );

            var e = myDocument.GetElementByPath( path );

            Assert.AreEqual( 0, e.GetRoot().GetChildPos() );
        }

        [Test]
        public void GetChildAt()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]" );

            var e = myDocument.GetElementByPath( path );

            Assert.AreEqual( "2,78", e.GetChildAt( "td", 1 ).InnerText );
            Assert.AreEqual( "3,00", e.GetChildAt( "td", 2 ).InnerText );
        }

        [Test]
        public void First()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]" );

            var element = myDocument.GetElementByPath( path );
            var body = element.Children.FirstOrDefault( e => e.TagName == "TBODY" );

            Assert.AreEqual( "TBODY", body.TagName );
        }

        [Test]
        public void FirstComplex()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]" );

            var element = myDocument.GetElementByPath( path );
            var first = element.Children.FirstOrDefault( e => e.TagName == "TD" && e.InnerText == "3,00" );

            Assert.AreEqual( "TD", first.TagName );
            Assert.AreEqual( "3,00", first.InnerText );
        }

        [Test]
        public void FindParent()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );

            var element = myDocument.GetElementByPath( path );
            var table = element.FindParent( e => e.TagName == "TABLE" );

            Assert.AreEqual( "TABLE", table.TagName );
        }

        [Test]
        public void FindParentWithAbort()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/CENTER[0]/TABLE[0]/TBODY[0]/TR[0]/TD[0]" );

            var element = myDocument.GetElementByPath( path );
            // dont leave the table
            var x = element.FindParent( e => e.TagName == "CENTER", e => e.TagName == "TABLE" );

            // if the abort condition fails "x" would not be null
            Assert.IsNull( x );
        }

        [Test]
        public void FindEmbeddingTable()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );

            var element = myDocument.GetElementByPath( path );
            HtmlTable table = element.FindEmbeddingTable();

            Assert.IsNotNull( table );
        }

        [Test]
        public void FindEmbeddingTableSelfEmbedded()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]" );

            var element = myDocument.GetElementByPath( path );
            HtmlTable table = element.FindEmbeddingTable();

            Assert.IsNotNull( table );

            table = element.Parent.FindEmbeddingTable();

            Assert.IsNotNull( table );
            Assert.AreEqual( element.Parent, table.TableElement );
        }

        [Test]
        public void FindEmbeddingTableNoTable()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]" );

            var element = myDocument.GetElementByPath( path );
            HtmlTable table = element.FindEmbeddingTable();

            Assert.IsNull( table );
        }

        [Test]
        public void IsTableOrTBody()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]" );
            var element = myDocument.GetElementByPath( path );
            Assert.IsTrue( element.IsTableOrTBody() );

            path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]" );
            element = myDocument.GetElementByPath( path );
            Assert.IsTrue( element.IsTableOrTBody() );

            path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );
            element = myDocument.GetElementByPath( path );
            Assert.IsFalse( element.IsTableOrTBody() );
        }

        [Test]
        public void GetElementByPath()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );

            var e = myDocument.GetElementByPath( path );

            Assert.AreEqual( "TD", e.TagName );
            Assert.AreEqual( "TR", e.Parent.TagName );
            Assert.AreEqual( 1, e.GetChildPos() );
            Assert.AreEqual( 6, e.Parent.GetChildPos() );
        }

        [Test]
        public void GetTextByPath()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );

            var e = myDocument.GetElementByPath( path );
            string value = myDocument.GetTextByPath( path );

            Assert.AreEqual( e.InnerText, value );
            Assert.AreEqual( "2,78", value );
        }

        [Test]
        public void GetTextByPath_SimplePath()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[0]/DIV[1]" );

            var e = myDocument.GetElementByPath( path );
            string value = myDocument.GetTextByPath( path );

            Assert.AreEqual( "Willkommen, Gast!", value );
        }

        [Test]
        public void GetTableByPath()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );

            var e = myDocument.GetElementByPath( path );
            e = e.Parent.Parent.Parent;
            HtmlTable table = myDocument.GetTableByPath( path );

            Assert.AreEqual( e, table.TableElement );
            Assert.AreEqual( e.Children.First(), table.TableBody );
        }
    }
}
