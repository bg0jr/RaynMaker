using System.Linq;
using NUnit.Framework;
using RaynMaker.Import.Html;

namespace RaynMaker.Import.Tests.Html
{
    [TestFixture]
    [RequiresSTA]
    public class HtmlElementExtensionsTests : TestBase
    {
        private IHtmlDocument myDocument = null;

        [TestFixtureSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            myDocument = LoadDocument( "ariva.html" );
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
    }
}
