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
    public class NavigatingHtmlDocumentScenarios : TestBase
    {
        private IHtmlDocument myDocument = null;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            myDocument = LoadDocument<IHtmlDocument>( "Html", "ariva.fundamentals.DE0005190003.html" );
        }

        [Test]
        public void Table_Create()
        {
            var path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );

            var table = HtmlTable.GetByPath( myDocument, path );

            Assert.AreEqual( "TABLE", table.TableElement.TagName );
            Assert.AreEqual( 9, table.Rows.Count() );
        }

        [Test]
        public void Table_GetIndices()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );

            var e = myDocument.GetElementByPath( path );
            var table = HtmlTable.GetByPath( myDocument, path );

            Assert.AreEqual( 1, table.GetColumnIndex( e ) );
            Assert.AreEqual( 6, table.GetRowIndex( e ) );
            Assert.AreEqual( e, table.GetEmbeddingTD( e ) );
            Assert.AreEqual( e.Parent, table.GetEmbeddingTR( e ) );
        }

        [Test]
        public void Table_GetCellAt()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );
            var e = myDocument.GetElementByPath( path );
            var table = HtmlTable.GetByPath( myDocument, path );

            Assert.AreEqual( e, table.GetCellAt( 6, 1 ) );
        }

        [Test]
        public void Table_GetRow()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );
            var e = myDocument.GetElementByPath( path );
            var table = HtmlTable.GetByPath( myDocument, path );

            var row = table.GetRow( e );

            Assert.AreEqual( 7, row.Count() );
            Assert.AreEqual( e, row.ElementAt( 1 ) );
        }

        [Test]
        public void Table_GetColumn()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );
            var e = myDocument.GetElementByPath( path );
            var table = HtmlTable.GetByPath( myDocument, path );

            var column = table.GetColumn( e );

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
        public void FindByCell()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );

            var element = myDocument.GetElementByPath( path );
            var table = HtmlTable.GetByElement( element );

            Assert.IsNotNull( table );
        }

        [Test]
        public void FindEmbeddingTableSelfEmbedded()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]" );

            var element = myDocument.GetElementByPath( path );
            var table = HtmlTable.GetByElement( element );

            Assert.IsNotNull( table );

            table = HtmlTable.GetByElement( element.Parent );

            Assert.IsNotNull( table );
            Assert.AreEqual( element.Parent, table.TableElement );
        }

        [Test]
        public void FindEmbeddingTableNoTable()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]" );

            var element = myDocument.GetElementByPath( path );
            var table = HtmlTable.GetByElement( element );

            Assert.IsNull( table );
        }

        [Test]
        public void IsTableOrTBody()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]" );
            var element = myDocument.GetElementByPath( path );
            Assert.IsTrue( HtmlTable.IsTableOrTBody( element ) );

            path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]" );
            element = myDocument.GetElementByPath( path );
            Assert.IsTrue( HtmlTable.IsTableOrTBody( element ) );

            path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );
            element = myDocument.GetElementByPath( path );
            Assert.IsFalse( HtmlTable.IsTableOrTBody( element ) );
        }

        [Test]
        public void GetElementByPath()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );

            var e = myDocument.GetElementByPath( path );

            Assert.AreEqual( "TD", e.TagName );
            Assert.AreEqual( "TR", e.Parent.TagName );
        }


        [Test]
        public void GetElementByPath_SimplePath()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[0]/DIV[1]" );

            var e = myDocument.GetElementByPath( path );

            Assert.AreEqual( "Willkommen, Gast!", e.InnerText );
        }

        [Test]
        public void GetTableByPath()
        {
            HtmlPath path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );

            var e = myDocument.GetElementByPath( path );
            e = e.Parent.Parent.Parent;
            var table = HtmlTable.GetByPath( myDocument, path );

            Assert.AreEqual( e, table.TableElement );
        }
    }
}
