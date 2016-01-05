using System.Drawing;
using NUnit.Framework;
using RaynMaker.Modules.Import.Design;

namespace RaynMaker.Modules.Import.UnitTests.Design
{
    [RequiresSTA]
    [TestFixture]
    class HtmlTableMarkerTests : HtmlMarkupTestBase
    {
        protected override string GetHtml()
        {
            return @"
<html>
    <body>
        <table>
            <thead>
                <tr>
                    <th id='c00'/>
                    <th id='c01'>Column 1</th>
                    <th id='c02'>Column 2</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <th id='c10'>Row 1</th>
                    <td id='c11'>v 11</td>
                    <td id='c12'>v 12</td>
                </tr>
                <tr>
                    <th id='c20'>Row 2</th>
                    <td id='c21'>v 21</td>
                    <td id='c22'>v 22</td>
                </tr>
            </tbody>
        </table>
    </body>
</html>
";
        }

        private HtmlTableMarker myMarker;

        [SetUp]
        public void SetUp()
        {
            myMarker = new HtmlTableMarker( Color.Yellow, Color.SteelBlue );
            ShowMarkupResultInBrowser = true;
        }

        [TearDown]
        public void TearDown()
        {
            myMarker.Reset();
            myMarker = null;
        }

        [Test]
        public void Mark_SingleCell()
        {
            var cell21 = Document.GetElementById( "c21" );
            myMarker.Mark( cell21 );

            Assert_IsMarked( cell21 );

            myMarker.Unmark();

            Assert_IsUnmarked( cell21, null );
        }

        [Test]
        public void Mark_ExpandRow()
        {
            myMarker.Mark( Document.GetElementById( "c21" ) );

            myMarker.ExpandRow = true;

            Assert_IsMarked( "c20", "c21", "c22" );

            myMarker.Unmark();

            Assert_IsUnmarked( "c20", "c21", "c22" );
        }

        [Test]
        public void ExpandRow_Mark()
        {
            myMarker.ExpandRow = true;

            myMarker.Mark( Document.GetElementById( "c21" ) );

            Assert_IsMarked( "c20", "c21", "c22" );

            myMarker.Unmark();

            Assert_IsUnmarked( "c20", "c21", "c22" );
        }

        [Test]
        public void Mark_ExpandColumn()
        {
            myMarker.Mark( Document.GetElementById( "c21" ) );

            myMarker.ExpandColumn = true;

            Assert_IsMarked( "c01", "c11", "c21" );

            myMarker.Unmark();

            Assert_IsUnmarked( "c01", "c11", "c21" );
        }

        [Test]
        public void ExpandColumn_Mark()
        {
            myMarker.ExpandColumn = true;

            myMarker.Mark( Document.GetElementById( "c21" ) );

            Assert_IsMarked( "c01", "c11", "c21" );

            myMarker.Unmark();

            Assert_IsUnmarked( "c01", "c11", "c21" );
        }
    }
}
