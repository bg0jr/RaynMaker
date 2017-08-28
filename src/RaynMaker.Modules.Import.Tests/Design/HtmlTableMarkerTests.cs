using System.Drawing;
using System.Threading;
using NUnit.Framework;
using RaynMaker.Modules.Import.Design;

namespace RaynMaker.Modules.Import.Tests.Design
{
    [Apartment(ApartmentState.STA)]
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
                    <th id='c03'>Column 3</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <th id='c10'>Row 1</th>
                    <td id='c11'>v 11</td>
                    <td id='c12'>v 12</td>
                    <td id='c13'>v 13</td>
                </tr>
                <tr>
                    <th id='c20'>Row 2</th>
                    <td id='c21'>v 21</td>
                    <td id='c22'>v 22</td>
                    <td id='c23'>v 23</td>
                </tr>
                <tr>
                    <th id='c30'>Row 3</th>
                    <td id='c31'>v 31</td>
                    <td id='c32'>v 32</td>
                    <td id='c33'>v 33</td>
                </tr>
            </tbody>
        </table>

        <table>
            <tr>
                <td id='x11'>vx 11</td>
                <td id='x12'>vx 12</td>
            </tr>
            <tr>
                <td id='x21'>vx 21</td>
                <td id='x22'>vx 22</td>
            </tr>
        </table>

        <p id='p1'>Not a table</p>
    </body>
</html>
";
        }

        private HtmlTableMarker myMarker;

        [SetUp]
        public void SetUp()
        {
            myMarker = new HtmlTableMarker( Color.Yellow, Color.SteelBlue );
        }

        [TearDown]
        public void TearDown()
        {
            myMarker.Reset();
            myMarker = null;
        }

        /// <summary>
        /// Rest of this marker implementation cannot handle the case that the selected element
        /// is not within a table.
        /// </summary>
        [Test]
        public void Mark_NotACell_ElementIsIgnored()
        {
            var element = Document.GetElementById( "p1" );
            myMarker.Mark( element );

            Assert_IsUnmarked( element, null );
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

        [Test]
        public void Mark_RowHeaderColumn()
        {
            myMarker.Mark( Document.GetElementById( "c21" ) );

            myMarker.ExpandRow = true;
            myMarker.RowHeaderColumn = 0;

            Assert_IsMarked( myMarker.HeaderColor, "c20" );

            myMarker.Unmark();

            Assert_IsUnmarked( "c20" );
        }

        [Test]
        public void RowHeaderColumn_Mark()
        {
            myMarker.ExpandRow = true;
            myMarker.RowHeaderColumn = 0;

            myMarker.Mark( Document.GetElementById( "c21" ) );

            Assert_IsMarked( myMarker.HeaderColor, "c20" );

            myMarker.Unmark();

            Assert_IsUnmarked( "c20" );
        }

        [Test]
        public void Mark_ColumnHeaderRow()
        {
            myMarker.Mark( Document.GetElementById( "c21" ) );

            myMarker.ExpandColumn = true;
            myMarker.ColumnHeaderRow = 0;

            Assert_IsMarked( myMarker.HeaderColor, "c01" );

            myMarker.Unmark();

            Assert_IsUnmarked( "c01" );
        }

        [Test]
        public void ColumnHeaderRow_Mark()
        {
            myMarker.ExpandColumn = true;
            myMarker.ColumnHeaderRow = 0;

            myMarker.Mark( Document.GetElementById( "c21" ) );

            Assert_IsMarked( myMarker.HeaderColor, "c01" );

            myMarker.Unmark();

            Assert_IsUnmarked( "c01" );
        }

        [Test]
        public void Mark_ColumnHeaderRowAndRowHeaderColumn()
        {
            myMarker.Mark( Document.GetElementById( "c21" ) );

            myMarker.ColumnHeaderRow = 0;
            myMarker.RowHeaderColumn = 0;

            Assert_IsMarked( myMarker.HeaderColor, "c01", "c20" );

            myMarker.Unmark();

            Assert_IsUnmarked( "c01", "c20" );
        }

        [Test]
        public void ColumnHeaderRowAndRowHeaderColumn_Mark()
        {
            myMarker.ColumnHeaderRow = 0;
            myMarker.RowHeaderColumn = 0;

            myMarker.Mark( Document.GetElementById( "c21" ) );

            Assert_IsMarked( myMarker.HeaderColor, "c01", "c20" );

            myMarker.Unmark();

            Assert_IsUnmarked( "c01", "c20" );
        }

        [Test]
        public void Mark_SkipColumnAndRows()
        {
            myMarker.Mark( Document.GetElementById( "c22" ) );

            myMarker.ExpandColumn = true;
            myMarker.ExpandRow = true;
            myMarker.ColumnHeaderRow = 0;
            myMarker.RowHeaderColumn = 0;
            myMarker.SkipRows = new[] { 1 };
            myMarker.SkipColumns = new[] { 1 };

            Assert_IsMarked( myMarker.HeaderColor, "c00", "c02", "c03" );
            Assert_IsMarked( myMarker.HeaderColor, "c00", "c20", "c30" );
            Assert_IsMarked( myMarker.CellColor, "c22", "c23", "c32" );

            myMarker.Unmark();

            Assert_IsUnmarked( "c00", "c02", "c03" );
            Assert_IsUnmarked( "c00", "c20", "c30" );
            Assert_IsUnmarked( "c22", "c23", "c32" );
        }

        [Test]
        public void SkipColumnAndRows_Mark()
        {
            myMarker.ExpandColumn = true;
            myMarker.ExpandRow = true;
            myMarker.ColumnHeaderRow = 0;
            myMarker.RowHeaderColumn = 0;
            myMarker.SkipRows = new[] { 1 };
            myMarker.SkipColumns = new[] { 1 };

            myMarker.Mark( Document.GetElementById( "c22" ) );

            Assert_IsMarked( myMarker.HeaderColor, "c00", "c02", "c03" );
            Assert_IsMarked( myMarker.HeaderColor, "c00", "c20", "c30" );
            Assert_IsMarked( myMarker.CellColor, "c22", "c23", "c32" );

            myMarker.Unmark();

            Assert_IsUnmarked( "c00", "c02", "c03" );
            Assert_IsUnmarked( "c00", "c20", "c30" );
            Assert_IsUnmarked( "c22", "c23", "c32" );
        }

        [Test]
        public void Mark_WhenAlreadyOtherTableIsMarked_OldTableIsUnmarked()
        {
            myMarker.ExpandRow = true;
            myMarker.RowHeaderColumn = 0;

            myMarker.Mark( Document.GetElementById( "x22" ) );

            Assert_IsMarked( myMarker.HeaderColor, "x21" );
            Assert_IsMarked( myMarker.CellColor, "x22" );

            myMarker.Mark( Document.GetElementById( "c21" ) );

            Assert_IsUnmarked( "x21", "x22" );
        }

        [Test]
        public void Reset_WhenCalled_AllPropertiesResetted()
        {
            myMarker.Mark( Document.GetElementById( "c22" ) );

            myMarker.ExpandColumn = true;
            myMarker.ExpandRow = true;
            myMarker.ColumnHeaderRow = 0;
            myMarker.RowHeaderColumn = 0;
            myMarker.SkipRows = new[] { 1 };
            myMarker.SkipColumns = new[] { 1 };

            myMarker.Reset();

            Assert.That( myMarker.ExpandColumn, Is.False );
            Assert.That( myMarker.ExpandRow, Is.False );
            Assert.That( myMarker.ColumnHeaderRow, Is.EqualTo( -1 ) );
            Assert.That( myMarker.RowHeaderColumn, Is.EqualTo( -1 ) );
            Assert.That( myMarker.SkipRows, Is.Null );
            Assert.That( myMarker.SkipColumns, Is.Null );

            // just assert for one of the marked cells
            Assert_IsUnmarked( "c22" );
        }
    }
}
