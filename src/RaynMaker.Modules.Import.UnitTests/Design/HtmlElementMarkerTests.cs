using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using NUnit.Framework;
using RaynMaker.Modules.Import.Design;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Documents.WinForms;

namespace RaynMaker.Modules.Import.UnitTests.Design
{
    [RequiresSTA]
    [TestFixture]
    class HtmlElementMarkerTests : HtmlMarkupTestBase
    {
        protected override string GetHtml()
        {
            return @"
<html>
    <body>
        <div id='a2'>
            <p id='a1'>
                Some text with <b>bold</b> parts
            </p>
            <p id='a3'>
                second paragraph
            </p>
        </div>

        <ul id='a4'>
            <li id='a5'>bullet one</li>
            <li id='a6'>bullet two</li>
        </ul>

        <p id='a7' style='color:red;background-color:blue;font-size:300%'>
            with existing style
        </p>

        <table>
            <tbody id='a8'>
                <tr>
                    <td>c1</td>
                    <td>c2</td>
                </tr>
                <tr>
                    <td id='a9'>c3</td>
                    <td>c4</td>
                </tr>
            </tbody>
        </table>
    </body>
</html>
";
        }

        private HtmlElementMarker myMarker;

        [SetUp]
        public void SetUp()
        {
            myMarker = new HtmlElementMarker( Color.Yellow );
        }

        [TearDown]
        public void TearDown()
        {
            myMarker.Reset();
            myMarker = null;
        }

        [Test]
        public void Mark_ParagraphWithInnerTags()
        {
            var element = Document.GetElementById( "a1" );

            myMarker.Mark( element );
            Assert_IsMarked( element );

            myMarker.Unmark();
            Assert_IsUnmarked( element, null );
        }

        [Test]
        public void Mark_DivWithTwoParagraphs()
        {
            var element = Document.GetElementById( "a2" );

            myMarker.Mark( element );
            Assert_IsMarked( element );

            myMarker.Unmark();
            Assert_IsUnmarked( element, null );
        }

        [Test]
        public void Mark_OneBulletOfList()
        {
            var element = Document.GetElementById( "a5" );

            myMarker.Mark( element );
            Assert_IsMarked( element );

            myMarker.Unmark();
            Assert_IsUnmarked( element, null );
        }

        [Test]
        public void Mark_WholeList()
        {
            var element = Document.GetElementById( "a4" );

            myMarker.Mark( element );
            Assert_IsMarked( element );

            myMarker.Unmark();
            Assert_IsUnmarked( element, null );
        }

        [Test]
        public void Mark_ElementWithExistingStyle()
        {
            var element = Document.GetElementById( "a7" );

            myMarker.Mark( element );
            Assert_IsMarked( element );

            myMarker.Unmark();
            Assert_IsUnmarked( element, "background-color: blue" );
        }

        [Test]
        public void Mark_EntireTable()
        {
            var element = Document.GetElementById( "a8" );

            myMarker.Mark( element );
            Assert_IsMarked( element );

            myMarker.Unmark();
            Assert_IsUnmarked( element, null );
        }

        [Test]
        public void Mark_SingleTableCell()
        {
            var element = Document.GetElementById( "a9" );

            myMarker.Mark( element );
            Assert_IsMarked( element );

            myMarker.Unmark();
            Assert_IsUnmarked( element, null );
        }

        [Test]
        public void Unmark_MultipleMarkersUnmarkedInReversedOrder_RollbackToPreviousStyle()
        {
            var element = Document.GetElementById( "a3" );

            var yellowMarker = new HtmlElementMarker( Color.Yellow );
            yellowMarker.Mark( element );
            Assert.That( element.Style, Is.StringContaining( "background-color: yellow" ).IgnoreCase );

            var blueMarker = new HtmlElementMarker( Color.Blue );
            blueMarker.Mark( element );
            Assert.That( element.Style, Is.StringContaining( "background-color: blue" ).IgnoreCase );

            var greenMarker = new HtmlElementMarker( Color.Green );
            greenMarker.Mark( element );
            Assert.That( element.Style, Is.StringContaining( "background-color: green" ).IgnoreCase );

            greenMarker.Unmark();
            Assert.That( element.Style, Is.StringContaining( "background-color: blue" ).IgnoreCase );

            blueMarker.Unmark();
            Assert.That( element.Style, Is.StringContaining( "background-color: yellow" ).IgnoreCase );

            yellowMarker.Unmark();
            Assert.That( element.Style, Is.Null );
        }

        [Test]
        public void Unmark_MultipleMarkersUnmarkedInSameOrder_LastStyleKeptUntilLastMarkerIsRemoved()
        {
            var element = Document.GetElementById( "a3" );

            var yellowMarker = new HtmlElementMarker( Color.Yellow );
            yellowMarker.Mark( element );
            Assert.That( element.Style, Is.StringContaining( "background-color: yellow" ).IgnoreCase );

            var blueMarker = new HtmlElementMarker( Color.Blue );
            blueMarker.Mark( element );
            Assert.That( element.Style, Is.StringContaining( "background-color: blue" ).IgnoreCase );

            var greenMarker = new HtmlElementMarker( Color.Green );
            greenMarker.Mark( element );
            Assert.That( element.Style, Is.StringContaining( "background-color: green" ).IgnoreCase );

            yellowMarker.Unmark();
            Assert.That( element.Style, Is.StringContaining( "background-color: green" ).IgnoreCase );

            blueMarker.Unmark();
            Assert.That( element.Style, Is.StringContaining( "background-color: green" ).IgnoreCase );

            greenMarker.Unmark();
            Assert.That( element.Style, Is.Null );
        }
    }
}
