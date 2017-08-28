using NUnit.Framework;
using RaynMaker.Modules.Import.Parsers.Html;

namespace RaynMaker.Modules.Import.Tests.Html
{
    [TestFixture]
    class HtmlPathTests
    {
        [Test]
        public void PointsToTable_NoElements_ReturnsFalse()
        {
            var path = new HtmlPath();

            Assert.That( path.PointsToTable, Is.False );
        }

        [Test]
        public void PointsToTableCell_NoElements_ReturnsFalse()
        {
            var path = new HtmlPath();

            Assert.That( path.PointsToTableCell, Is.False );
        }

        [Test]
        public void GetPathToTable_PathToTDWithTBody_ReturnsPathToTableElement()
        {
            var path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]" );

            var pathToTable = path.GetPathToTable();

            Assert.That( pathToTable.ToString(), Is.EqualTo( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]" ) );
        }

        [Test]
        public void GetPathToTable_PathToTDWithoutTBody_ReturnsPathToTableElement()
        {
            var path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TR[6]/TD[1]" );

            var pathToTable = path.GetPathToTable();

            Assert.That( pathToTable.ToString(), Is.EqualTo( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]" ) );
        }

        [Test]
        public void GetPathToTable_PathToTBody_ReturnsPathToTBodyElement()
        {
            var path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]" );

            var pathToTable = path.GetPathToTable();

            Assert.That( pathToTable.ToString(), Is.EqualTo( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]" ) );
        }

        [Test]
        public void GetPathToTable_PathToTable_ReturnsPathToTableElement()
        {
            var path = HtmlPath.Parse( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]" );

            var pathToTable = path.GetPathToTable();

            Assert.That( pathToTable.ToString(), Is.EqualTo( "/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]" ) );
        }

        [Test]
        public void Parse_RootOnly_ElementsAreEmpty()
        {
            var path = HtmlPath.Parse( "/" );

            Assert.That( path.Elements, Is.Empty );
        }

        /// <summary>
        /// As there is always only one singe HTML element (the root) we can omit it.
        /// </summary>
        [Test]
        public void Parse_FirstElementIsHtmlTag_ElementsArEmpty()
        {
            var path = HtmlPath.Parse( "/html[0]" );

            Assert.That( path.Elements, Is.Empty );
        }
    }
}
