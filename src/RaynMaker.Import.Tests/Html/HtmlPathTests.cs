using NUnit.Framework;
using RaynMaker.Import.Html;

namespace RaynMaker.Import.Tests.Html
{
    [TestFixture]
    public class HtmlPathTests
    {
        [Test]
        public void SimpleElement()
        {
            HtmlPathElement e = new HtmlPathElement( "Tr", 3 );

            Assert.AreEqual( "TR", e.TagName );
            Assert.AreEqual( 3, e.Position );
            Assert.IsFalse( e.IsTableOrTBody );
            Assert.AreEqual( "TR[3]", e.ToString() );
        }

        [Test]
        public void ElementParse()
        {
            HtmlPathElement e = HtmlPathElement.Parse( "table[2]" );

            Assert.AreEqual( "TABLE", e.TagName );
            Assert.AreEqual( 2, e.Position );
            Assert.IsTrue( e.IsTableOrTBody );
        }

        [Test]
        public void TableElement()
        {
            HtmlPathElement e = new HtmlPathElement( "Table", 3 );

            Assert.IsTrue( e.IsTableOrTBody );

            e = new HtmlPathElement( "Tbody", 3 );

            Assert.IsTrue( e.IsTableOrTBody );
        }

        [Test]
        public void PathElementWithWrongInput()
        {
            try
            {
                HtmlPathElement e = new HtmlPathElement( null, 0 );
                Assert.Fail( "It should not possible to create a HtmlPathElement with empty TagName" );
            }
            catch { }

            try
            {
                HtmlPathElement e = new HtmlPathElement( "  ", 0 );
                Assert.Fail( "It should not possible to create a HtmlPathElement with empty TagName" );
            }
            catch { }

            try
            {
                HtmlPathElement e = new HtmlPathElement( "TR", -1 );
                Assert.Fail( "It should not possible to create a HtmlPathElement with negative position" );
            }
            catch { }
        }

        [Test]
        public void SimplePath()
        {
            HtmlPath p = new HtmlPath();
            p.Elements.Add( new HtmlPathElement( "body", 0 ) );
            p.Elements.Add( new HtmlPathElement( "h3", 0 ) );

            Assert.AreEqual( 2, p.Elements.Count );
            Assert.IsFalse( p.PointsToTableCell );
            Assert.IsFalse( p.PointsToTable );
            Assert.AreEqual( "H3", p.Last.TagName );
            Assert.AreEqual( "/BODY[0]/H3[0]", p.ToString() );
        }

        [Test]
        public void TableCellPath()
        {
            HtmlPath p = new HtmlPath();
            p.Elements.Add( new HtmlPathElement( "table", 0 ) );
            p.Elements.Add( new HtmlPathElement( "tr", 2 ) );
            p.Elements.Add( new HtmlPathElement( "td", 4 ) );

            Assert.AreEqual( 3, p.Elements.Count );
            Assert.IsTrue( p.PointsToTableCell );
            Assert.IsFalse( p.PointsToTable );
            Assert.AreEqual( "TD", p.Last.TagName );
            Assert.AreEqual( "/TABLE[0]/TR[2]/TD[4]", p.ToString() );
        }

        [Test]
        public void TablePath()
        {
            HtmlPath p = new HtmlPath();
            p.Elements.Add( new HtmlPathElement( "table", 1 ) );

            Assert.AreEqual( 1, p.Elements.Count );
            Assert.IsFalse( p.PointsToTableCell );
            Assert.IsTrue( p.PointsToTable );
            Assert.AreEqual( "TABLE", p.Last.TagName );
            Assert.AreEqual( "/TABLE[1]", p.ToString() );
        }

        [Test]
        public void PathParse()
        {
            HtmlPath p = HtmlPath.Parse( "/TABLE[0]/TR[2]/TD[4]" );

            Assert.AreEqual( 3, p.Elements.Count );
            Assert.IsTrue( p.PointsToTableCell );
            Assert.IsFalse( p.PointsToTable );
            Assert.AreEqual( "TD", p.Last.TagName );
        }
    }
}
