using System.Diagnostics;
using System.IO;
using System.Text;
using NUnit.Framework;
using RaynMaker.Modules.Import.Design;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Documents.WinForms;

namespace RaynMaker.Modules.Import.UnitTests.Design
{
    [RequiresSTA]
    abstract class HtmlMarkupTestBase
    {
        protected static bool ShowMarkupResultInBrowser = false;

        private SafeWebBrowser myBrowser;
        protected IHtmlDocument Document { get; private set; }

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            myBrowser = new SafeWebBrowser();
            myBrowser.DownloadControlFlags = DocumentLoaderFactory.DownloadControlFlags;

            var html = GetHtml();
            myBrowser.DocumentText = html;
            myBrowser.Document.OpenNew( true );
            myBrowser.Document.Write( html );
            myBrowser.Refresh();

            Document = new HtmlDocumentAdapter( myBrowser.Document );
        }

        protected abstract string GetHtml();

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            myBrowser.Dispose();
        }

        protected void Assert_IsMarked( IHtmlElement element )
        {
            Assert.That( element.Style, Is.StringContaining( "background-color: yellow" ).IgnoreCase );

            if( ShowMarkupResultInBrowser )
            {
                ShowMarkedDocument();
            }
        }

        protected void Assert_IsUnmarked( IHtmlElement element, string originalStyle )
        {
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
        
        protected void ShowMarkedDocument()
        {
            myBrowser.Document.Title = TestContext.CurrentContext.Test.Name;

            var file = Path.GetTempFileName() + ".html";

            File.WriteAllText( file, myBrowser.Document.Body.Parent.OuterHtml, Encoding.GetEncoding( myBrowser.Document.Encoding ) );

            Process.Start( file ).WaitForExit();

            File.Delete( file );
        }
    }
}
