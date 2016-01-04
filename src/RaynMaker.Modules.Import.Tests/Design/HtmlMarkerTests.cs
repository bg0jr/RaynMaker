using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using NUnit.Framework;
using RaynMaker.Modules.Import.Design;
using RaynMaker.Modules.Import.Documents;

namespace RaynMaker.Modules.Import.UnitTests.Design
{
    [RequiresSTA]
    [TestFixture]
    class HtmlMarkerTests
    {
        private const string HtmlContent = @"
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

        private static bool ShowMarkupResultInBrowser = false;

        private SafeWebBrowser myBrowser;
        private HtmlElementMarker myMarker;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            myBrowser = new SafeWebBrowser();
            myBrowser.DownloadControlFlags = DocumentLoaderFactory.DownloadControlFlags;

            myBrowser.DocumentText = HtmlContent;
            myBrowser.Document.OpenNew( true );
            myBrowser.Document.Write( HtmlContent );
            myBrowser.Refresh();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            myBrowser.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            myMarker = new HtmlElementMarker();
        }

        [TearDown]
        public void TearDown()
        {
            myMarker.UnmarkAll();
            myMarker = null;
        }

        [Test]
        public void ParagraphWithInnerTags()
        {
            var element = myBrowser.Document.GetElementById( "a1" );

            myMarker.Mark( element );
            Assert_IsMarked( element );

            myMarker.Unmark( element );
            Assert_IsUnmarked( element, null );
        }

        [Test]
        public void DivWithTwoParagraphs()
        {
            var element = myBrowser.Document.GetElementById( "a2" );

            myMarker.Mark( element );
            Assert_IsMarked( element );

            myMarker.Unmark( element );
            Assert_IsUnmarked( element, null );
        }

        [Test]
        public void OneBulletOfList()
        {
            var element = myBrowser.Document.GetElementById( "a5" );

            myMarker.Mark( element );
            Assert_IsMarked( element );

            myMarker.Unmark( element );
            Assert_IsUnmarked( element, null );
        }

        [Test]
        public void WholeList()
        {
            var element = myBrowser.Document.GetElementById( "a4" );

            myMarker.Mark( element );
            Assert_IsMarked( element );

            myMarker.Unmark( element );
            Assert_IsUnmarked( element, null );
        }

        [Test]
        public void ElementWithExistingStyle()
        {
            var element = myBrowser.Document.GetElementById( "a7" );

            myMarker.Mark( element );
            Assert_IsMarked( element );

            myMarker.Unmark( element );
            Assert_IsUnmarked( element, "background-color: blue" );
        }

        [Test]
        public void EntireTable()
        {
            var element = myBrowser.Document.GetElementById( "a8" );

            myMarker.Mark( element );
            Assert_IsMarked( element );

            myMarker.Unmark( element );
            Assert_IsUnmarked( element, null );
        }

        [Test]
        public void SingleTableCell()
        {
            var element = myBrowser.Document.GetElementById( "a9" );

            myMarker.Mark( element );
            Assert_IsMarked( element );

            myMarker.Unmark( element );
            Assert_IsUnmarked( element, null );
        }

        private void Assert_IsMarked( HtmlElement element )
        {
            Assert.That( myMarker.IsMarked( element ), Is.True );
            Assert.That( element.Style, Is.StringContaining( "background-color: yellow" ).IgnoreCase );

            if( ShowMarkupResultInBrowser )
            {
                ShowMarkedDocument();
            }
        }

        private void Assert_IsUnmarked( HtmlElement element, string originalStyle )
        {
            Assert.That( myMarker.IsMarked( element ), Is.False );

            if( originalStyle == null )
            {
                Assert.That( element.Style, Is.Null );
            }
            else
            {
                // we cannot check for exact match of original style because the different parameters get reordered
                Assert.That( element.Style, Is.StringContaining( originalStyle ).IgnoreCase );
            }

            if( ShowMarkupResultInBrowser )
            {
                ShowMarkedDocument();
            }
        }

        private void ShowMarkedDocument()
        {
            myBrowser.Document.Title = TestContext.CurrentContext.Test.Name;

            var file = Path.GetTempFileName() + ".html";

            File.WriteAllText( file, myBrowser.Document.Body.Parent.OuterHtml, Encoding.GetEncoding( myBrowser.Document.Encoding ) );

            Process.Start( file ).WaitForExit();

            File.Delete( file );
        }
    }
}
