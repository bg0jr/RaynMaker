using System.Drawing;
using System.Threading;
using NUnit.Framework;
using RaynMaker.Modules.Import.Design;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Documents.WinForms;
using RaynMaker.SDK.Html;

namespace RaynMaker.Modules.Import.UnitTests.Design
{
    [Apartment(ApartmentState.STA)]
    abstract class HtmlMarkupTestBase
    {
        protected bool ShowMarkupResultInBrowser = false;

        private SafeWebBrowser myBrowser;
        protected IHtmlDocument Document { get; private set; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            myBrowser = new SafeWebBrowser();
            myBrowser.DownloadControlFlags = DocumentLoaderFactory.DownloadControlFlags;

            myBrowser.LoadHtml( GetHtml() );

            Document = new HtmlDocumentAdapter( myBrowser.Document );
        }

        protected abstract string GetHtml();

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            myBrowser.Dispose();
        }

        protected void Assert_IsMarked( IHtmlElement element )
        {
            if( ShowMarkupResultInBrowser )
            {
                ShowMarkedDocument();
            }

            Assert.That( element.Style, Does.Contain( "background-color: yellow" ).IgnoreCase );
        }

        protected void Assert_IsMarked( params string[] elementIds )
        {
            Assert_IsMarked( Color.Yellow, elementIds );
        }

        protected void Assert_IsMarked( Color color, params string[] elementIds )
        {
            if( ShowMarkupResultInBrowser )
            {
                ShowMarkedDocument();
            }

            foreach( var id in elementIds )
            {
                var element = Document.GetElementById( id );
                Assert.That( element.Style, Does.Contain( "background-color: " + ColorTranslator.ToHtml( color ) ).IgnoreCase, "Element with Id='{0}' is not marked", id );
            }
        }

        protected void Assert_IsUnmarked( IHtmlElement element, string originalStyle )
        {
            if( ShowMarkupResultInBrowser )
            {
                ShowMarkedDocument();
            }

            if( originalStyle == null )
            {
                Assert.That( element.Style, Is.Null );
            }
            else
            {
                // we cannot check for exact match of original style because the different parameters get reordered
                Assert.That( element.Style, Does.Contain( originalStyle ).IgnoreCase );
            }
        }

        protected void Assert_IsUnmarked( params string[] elementIds )
        {
            if( ShowMarkupResultInBrowser )
            {
                ShowMarkedDocument();
            }

            foreach( var id in elementIds )
            {
                var element = Document.GetElementById( id );
                Assert.That( element.Style, Is.Null, "Element with Id='{0}' is still marked", id );
            }
        }

        protected void ShowMarkedDocument()
        {
            myBrowser.Document.Title = TestContext.CurrentContext.Test.Name;
            myBrowser.Document.OpenDocumentInExternalBrowser();
        }
    }
}
