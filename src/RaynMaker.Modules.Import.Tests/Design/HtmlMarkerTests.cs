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
    </body>
</html>
";

        private static bool ShowMarkupResultInBrowser = true;

        private SafeWebBrowser myBrowser;
        private HtmlMarker myMarker;

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
            myMarker = new HtmlMarker();
        }

        [TearDown]
        public void TearDown()
        {
            myMarker.UnmarkAll();
            myMarker = null;
        }

        [Test]
        public void Mark_ParagraphWithInnerTags_EntireParagraphTextMarked()
        {
            var element = myBrowser.Document.GetElementById( "a1" );

            myMarker.Mark( element );

            Assert_IsMarked( element );
        }

        [Test]
        public void Mark_DivWithTwoParagraphs_AllParagraphsMarked()
        {
            var element = myBrowser.Document.GetElementById( "a2" );

            myMarker.Mark( element );

            Assert_IsMarked( element );
        }

        [Test]
        public void Mark_OneBulletOfList_OnlySingleBulletMarked()
        {
            var element = myBrowser.Document.GetElementById( "a5" );

            myMarker.Mark( element );

            Assert_IsMarked( element );
        }

        [Test]
        public void Mark_WholeList_WholeListMarked()
        {
            var element = myBrowser.Document.GetElementById( "a4" );

            myMarker.Mark( element );

            Assert_IsMarked( element );
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
